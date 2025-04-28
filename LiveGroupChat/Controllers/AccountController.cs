using LiveGroupChat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiveGroupChat.Controllers;

public class AccountController : Controller{


[Route("/account/register")]
public IActionResult Register(){
    return View(new RegistrationViewModel());
}

[Route("/account/register")]
[HttpPost]
public IActionResult Register(RegistrationViewModel registration)
{
    if (!ModelState.IsValid)
    {
        Console.WriteLine("BŁĄD");
        return View(registration);
    }

    Console.WriteLine("DOBRZE");
    return RedirectToAction("Login", "Account"); 
}










[Route("/account/login")]
public IActionResult Login(){
    return View();
}


}
    
