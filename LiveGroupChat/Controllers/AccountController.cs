using LiveGroupChat.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LiveGroupChat.Controllers;

public class AccountController : Controller{

private AppDbContext _context;
private IHttpContextAccessor _httpContextAccessor;


public AccountController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
{
    _context = context;
    _httpContextAccessor = httpContextAccessor;
}

[Route("/account/login")]
public IActionResult Login(){
    return View();
}


[Route("/account/login")]
[HttpPost]
public IActionResult Login(string nickname)
{
    Console.BackgroundColor = ConsoleColor.Green;
    Console.WriteLine(nickname);

    var user = _context.Users.Add(new User() { Nickname =nickname });
    _context.SaveChanges();

    _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Entity.Id.ToString());

    return RedirectToAction("Home", "Home");
}


}