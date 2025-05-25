using LiveGroupChat.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LiveGroupChat.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    [Route("/account/login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [Route("/account/login")]
    public IActionResult Login(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
        {
            ModelState.AddModelError("nickname", "Nickname is required.");
            return View();
        }

        var user = new User { Nickname = nickname };
        _context.Users.Add(user);
        _context.SaveChanges();

        _httpContextAccessor.HttpContext?.Session.SetString("UserId", user.Id.ToString());

        return RedirectToAction("Home", "Home");
    }
}