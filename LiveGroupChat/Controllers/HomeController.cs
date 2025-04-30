using LiveGroupChat.Models;
using LiveGroupChat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiveGroupChat.Controllers;

public class HomeController : Controller{

    
    private static readonly List<MessageViewModel> messages = new List<MessageViewModel>();
    
    [Route("/home")]
    public ActionResult Home() {
        if (messages.Count > 5)messages.Clear();
        

        {
            
        }
        
        return View(messages);
    }
    [HttpPost]
    [Route("/home")]
        public ActionResult WriteMessage(MessageViewModel message)
        {
            
            
            Random random1= new Random();
            if (random1.Next(2) == 0)    message.User = new UserViewModel(){FirstName = "Jan", LastName = "Kowalski"};
            else message.User = new UserViewModel(){FirstName = "Maciej", LastName = "Nowak"};
            
            
       Random random= new Random();
        if (random.Next(2) == 0) message.Created = DateTime.Now.AddDays(-1);
        else message.Created = DateTime.Now;
        
        
        if (!string.IsNullOrWhiteSpace(message.Text)) 
        {
            messages.Add(message);
        }

        return RedirectToAction("Home", "Home");
    }

    
    
}