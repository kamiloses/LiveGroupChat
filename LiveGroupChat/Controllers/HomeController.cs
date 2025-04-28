using Microsoft.AspNetCore.Mvc;

namespace LiveGroupChat.Controllers;

public class HomeController : Controller{

    [Route("/home")]
    public ActionResult Home()
    {
        
        return View();
    }
}