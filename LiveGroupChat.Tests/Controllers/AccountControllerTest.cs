// using LiveGroupChat.Controllers;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Moq;
// using Xunit;
//
// namespace LiveGroupChat.Tests.Controllers;
// public class AccountControllerTests
// {
//     [Fact]
//     public void Login_Get_ReturnsView()
//     {
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase("AccDb1").Options;
//
//         using var ctx = new AppDbContext(options);
//         var ctrl = new AccountController(ctx, new Mock<IHttpContextAccessor>().Object);
//
//         var result = ctrl.Login();
//
//         Assert.IsType<ViewResult>(result);
//     }
//
//     [Fact]
//     public void Login_Post_AddsUser_AndRedirects()
//     {
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase("AccDb2").Options;
//
//         using var ctx = new AppDbContext(options);
//         var session = new Mock<ISession>();
//         var httpCtx = new Mock<HttpContext>();
//         httpCtx.Setup(c => c.Session).Returns(session.Object);
//         var accessor = new Mock<IHttpContextAccessor>();
//         accessor.Setup(a => a.HttpContext).Returns(httpCtx.Object);
//
//         var ctrl = new AccountController(ctx, accessor.Object);
//         var result = ctrl.Login("Tester");
//
//         Assert.IsType<RedirectToActionResult>(result);
//         Assert.Single(ctx.Users);
//     }
// }