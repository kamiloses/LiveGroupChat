using LiveGroupChat.Models.Entities;
using LiveGroupChat.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LiveGroupChat.Controllers;

public class AccountController : Controller{

private AppDbContext _context;
private IHttpContextAccessor _httpContextAccessor;
public User loggedUserData=new User();


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
public  IActionResult Login(String nickname)
    {
  EntityEntry<User> user =  _context.Users.Add(new User(){Nickname = "Abc"+Random.Shared.Next(0,100)});
  
  
  _context.SaveChanges();
     loggedUserData.Id = user.Entity.Id;
     loggedUserData.Nickname = nickname;
     _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Entity.Id.ToString());  

    
    return RedirectToAction("Home", "Home"); 
}


}