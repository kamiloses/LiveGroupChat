using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiveGroupChat.Models.Entities;
using LiveGroupChat.Repositories;
using LiveGroupChat.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LiveGroupChat.Tests.Services
{
    public class HomeServiceTests
    {
        private ISession MockSession(string userId)
        {
            var session = new Mock<ISession>();
            session.Setup(s => s.TryGetValue("UserId", out It.Ref<byte[]>.IsAny))
                .Returns((string key, out byte[] val) =>
                {
                    val = Encoding.UTF8.GetBytes(userId);
                    return true;
                });
            return session.Object;
        }

        private IHttpContextAccessor MockHttpContextAccessor(string userId)
        {
            var ctx = new Mock<HttpContext>();
            ctx.Setup(c => c.Session).Returns(MockSession(userId));
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(a => a.HttpContext).Returns(ctx.Object);
            return accessor.Object;
        }

        [Fact]
        public void GetAllMessages_CreatesUser_IfNotExists()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb1").Options;

            using var ctx = new AppDbContext(options);
            var messageRepo = new MessageRepository(ctx);
            var userRepo = new UserRepository(ctx);

            var service = new HomeService(messageRepo, userRepo, MockHttpContextAccessor("10"));

            service.GetAllMessages();

            Assert.True(ctx.Users.Any(u => u.Id == 10));
        }

        [Fact]
        public void GetAllMessages_RemovesMessages_When6OrMore()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb2").Options;

            using var ctx = new AppDbContext(options);
            ctx.Users.Add(new User { Id = 1 });
            for (int i = 0; i < 6; i++)
                ctx.Messages.Add(new Message { UserId = 1, Text = $"Msg{i}" });
            ctx.SaveChanges();

            var messageRepo = new MessageRepository(ctx);
            var userRepo = new UserRepository(ctx);
            var service = new HomeService(messageRepo, userRepo, MockHttpContextAccessor("1"));

            service.GetAllMessages();

            Assert.Empty(ctx.Messages);
        }

        [Fact]
        public void GetAllMessages_ReturnsAllMessages()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb3").Options;

            using var ctx = new AppDbContext(options);
            var user = new User { Id = 2, Nickname = "User2" };
            ctx.Users.Add(user);
            ctx.Messages.Add(new Message { Text = "Hello", UserId = 2 });
            ctx.SaveChanges();

            var messageRepo = new MessageRepository(ctx);
            var userRepo = new UserRepository(ctx);
            var service = new HomeService(messageRepo, userRepo, MockHttpContextAccessor("2"));

            var messages = service.GetAllMessages();

            Assert.Single(messages);
            Assert.Equal("Hello", messages[0].Text);
        }
    }
}
