using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiveGroupChat.Models.Entities;
using LiveGroupChat.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class HomeServiceTests
{
    private delegate void TryGetValueCallback(string key, out byte[] value);

    private ISession CreateMockSessionWithUserId(string userId)
    {
        var session = new Mock<ISession>();
        session.Setup(s => s.TryGetValue("UserId", out It.Ref<byte[]>.IsAny))
            .Callback(new TryGetValueCallback((string key, out byte[] value) =>
            {
                value = Encoding.UTF8.GetBytes(userId);
            }))
            .Returns(true);

        return session.Object;
    }

    private IHttpContextAccessor CreateMockHttpContextAccessor(string userId)
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Session).Returns(CreateMockSessionWithUserId(userId));

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        return mockHttpContextAccessor.Object;
    }

    [Fact]
    public void GetAllMessages_CreatesUser_WhenUserDoesNotExist()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb1")
            .Options;

        using var context = new AppDbContext(options);
        var service = new HomeService(context, CreateMockHttpContextAccessor("999"));

        var result = service.GetAllMessages();

        Assert.True(context.Users.Any(u => u.Id == 999));
    }

    [Fact]
    public void GetAllMessages_DeletesMessages_WhenCountIsSixOrMore()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb2")
            .Options;

        using var context = new AppDbContext(options);

        context.Users.Add(new User { Id = 1, Nickname = "TestUser" });
        for (int i = 0; i < 6; i++)
        {
            context.Messages.Add(new Message { Text = $"Message {i}", UserId = 1 });
        }
        context.SaveChanges();

        var service = new HomeService(context, CreateMockHttpContextAccessor("1"));

        var result = service.GetAllMessages();

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void GetAllMessages_ReturnsMessagesWithIncludes()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb3")
            .Options;

        using var context = new AppDbContext(options);

        var user = new User { Id = 2, Nickname = "IncludedUser" };
        var message = new Message
        {
            Text = "Test Message",
            UserId = 2,
            User = user,
            Reactions = new List<Reaction> { new Reaction { Emoji = "👍", UserId = 2, user = user } }
        };
        context.Users.Add(user);
        context.Messages.Add(message);
        context.SaveChanges();

        var service = new HomeService(context, CreateMockHttpContextAccessor("2"));

        var result = service.GetAllMessages();

        Assert.Single(result);
        Assert.Equal("Test Message", result[0].Text);
        Assert.NotNull(result[0].User);
        Assert.Single(result[0].Reactions);
    }
}
