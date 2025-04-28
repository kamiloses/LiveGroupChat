using Microsoft.AspNetCore.Mvc;

namespace LiveGroupChat.Controllers;

public class AccountController : Controller{


[Route("/account/register")]
public IActionResult Register(){
    return View();
}
[Route("/account/login")]
public IActionResult Login(){
    return View();
}


}
    
