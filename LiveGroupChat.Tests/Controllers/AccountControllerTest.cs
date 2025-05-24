using LiveGroupChat.Controllers;
using LiveGroupChat.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class AccountControllerSimpleTests
{
    [Fact]
    public void Login_Get_ReturnsView()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("LoginGetDb")
            .Options;

        using var context = new AppDbContext(options);
        var mockAccessor = new Mock<IHttpContextAccessor>();
        var controller = new AccountController(context, mockAccessor.Object);

        // Act
        var result = controller.Login();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Login_Post_AddsUser_AndRedirects()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("LoginPostDb")
            .Options;

        using var context = new AppDbContext(options);

        var mockSession = new Mock<ISession>();
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);

        var mockAccessor = new Mock<IHttpContextAccessor>();
        mockAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        var controller = new AccountController(context, mockAccessor.Object);

        var result = controller.Login("Tester");

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Single(context.Users); 
    }
}