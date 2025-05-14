using LiveGroupChat.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiveGroupChat.Controllers;

public class AccountController : Controller{




[Route("/account/login")]
public IActionResult Login(){
    return View();
}


[Route("/account/login")]
[HttpPost]
public IActionResult Login(LoginViewModel loginModel){
    return RedirectToAction("Home", "Home"); 
}


}