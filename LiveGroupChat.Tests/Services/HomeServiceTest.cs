using System.Text;
using System.Linq;
using LiveGroupChat.Models.Entities;
using LiveGroupChat.Repositories;
using LiveGroupChat.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace LiveGroupChat.Tests.Services
{
    public class HomeServiceTests
    {
        private ISession MockSession(string userId)
        {
            var session = new Mock<ISession>();
            byte[] userIdBytes = Encoding.UTF8.GetBytes(userId);

            session.Setup(s => s.TryGetValue("UserId", out userIdBytes)).Returns(true);

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
        public async Task GetAllMessages_CreatesUser_IfNotExists()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb1").Options;

            await using var ctx = new AppDbContext(options);
            var messageRepo = new MessageRepository(ctx);
            var userRepo = new UserRepository(ctx);

            var service = new HomeService(messageRepo, userRepo, MockHttpContextAccessor("10"));

            var messages = await service.GetAllMessagesAsync();

            Assert.True(ctx.Users.Any(u => u.Id == 10));
        }

        [Fact]
        public async Task GetAllMessages_RemovesMessages_When6OrMore()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb2").Options;

            await using var ctx = new AppDbContext(options);
            ctx.Users.Add(new User { Id = 1, Nickname = "TestUser" });  
            for (int i = 0; i < 6; i++)
                ctx.Messages.Add(new Message { UserId = 1, Text = $"Msg{i}" });
            await ctx.SaveChangesAsync();

            var messageRepo = new MessageRepository(ctx);
            var userRepo = new UserRepository(ctx);
            var service = new HomeService(messageRepo, userRepo, MockHttpContextAccessor("1"));

            await service.GetAllMessagesAsync();

            ctx.ChangeTracker.Clear();

            Assert.Empty(ctx.Messages);
        }

        [Fact]
        public async Task GetAllMessages_ReturnsAllMessages()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb3").Options;

            await using var ctx = new AppDbContext(options);
            var user = new User { Id = 2, Nickname = "User2" };
            ctx.Users.Add(user);
            ctx.Messages.Add(new Message { Text = "Hello", UserId = 2 });
            await ctx.SaveChangesAsync();

            var messageRepo = new MessageRepository(ctx);
            var userRepo = new UserRepository(ctx);
            var service = new HomeService(messageRepo, userRepo, MockHttpContextAccessor("2"));

            var messages = await service.GetAllMessagesAsync();

            Assert.Single(messages);
            Assert.Equal("Hello", messages[0].Text);
        }
    }
}
