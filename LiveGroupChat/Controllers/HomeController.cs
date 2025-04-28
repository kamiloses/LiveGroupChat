using LiveGroupChat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiveGroupChat.Controllers;

public class HomeController : Controller{

    
    private static readonly List<MessageViewModel> messages = new List<MessageViewModel>();
    
    [Route("/home")]
    public ActionResult Home() {
        
        return View(messages);
    }
    [HttpPost]
    [Route("/home")]
    public ActionResult WriteMessage(MessageViewModel message) {
        if (!string.IsNullOrWhiteSpace(message.Text)) 
        {
            messages.Add(message);
        }

        return RedirectToAction("Home", "Home");
    }

    
    
}