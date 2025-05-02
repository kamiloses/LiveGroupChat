using LiveGroupChat.Data;
using LiveGroupChat.Models;
using LiveGroupChat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiveGroupChat.Controllers;

public class HomeController : Controller{

    
    private static readonly Dictionary<string,List<MessageViewModel>> messages = new Dictionary<string, List<MessageViewModel>>();
    private User user=new User();
 
    
    [Route("/home")]
    public ActionResult Home() {
        String  userId = HttpContext.Session.GetString("UserId").Substring(0,4);
         
         
        Console.ForegroundColor = ConsoleColor.Yellow;
          user.Id=userId;

          if (!messages.ContainsKey(userId))
          {messages.Add(userId, new List<MessageViewModel>());
              
          }
          
          
          
        if (messages.Count > 5)messages.Clear();
        var allMessages = messages.Values.SelectMany(list => list).ToList();
        return View(allMessages);

       // return View(messages.Values);
    }
    [HttpPost]
    [Route("/home")]
    public ActionResult WriteMessage(MessageViewModel message)
    {
        String userId = HttpContext.Session.GetString("UserId").Substring(0, 4);

        Random random1 = new Random();
        message.Id=random1.Next(0,1000);
        if (random1.Next(2) == 0)  
            message.User = new UserViewModel() {Id=userId, FirstName = "Jan", LastName = "Kowalski" };
        else 
            message.User = new UserViewModel() {Id=userId, FirstName = "Maciej", LastName = "Nowak" };

        Random random = new Random();
        if (random.Next(2) == 0) 
            message.Created = DateTime.Now.AddDays(-1);
        else 
            message.Created = DateTime.Now;

        // Upewnij się, że klucz istnieje w słowniku przed próbą dodania wiadomości
        if (!messages.ContainsKey(userId))
        {
            messages.Add(userId, new List<MessageViewModel>());
        }

        if (!string.IsNullOrWhiteSpace(message.Text))
        {
            // Dodaj wiadomość do listy w słowniku
            messages[userId].Add(message);
             
            
        }
 
        return RedirectToAction("Home", "Home");
    }


        int number = 0;

        [HttpPost]
        [Route("/home/emoji")]
        public ActionResult AddEmoji(int id, string reaction)
        {
            number++;

            String userId = HttpContext.Session.GetString("UserId").Substring(0, 4);

            int reactionId = new Random().Next();
            Reaction newReaction = new Reaction();
            newReaction.Id = reactionId;
            newReaction.Emoji = reaction;
            newReaction.Number = number;
            newReaction.MessageId = id;
            newReaction.UserId = userId;


            var message = messages[userId].Single(message => message.Id == id);
                
           bool a= message.Reactions.Any(reaction => reaction.UserId == userId);
              Console.WriteLine(a);
           if (!a)
           {
               message.Reactions.Add(newReaction);
              Console.Write("zrobione");               
           }
           else
           {
               var reactionToRemove = message.Reactions.Single(reaction => reaction.UserId == userId);
               message.Reactions.Remove(reactionToRemove);
               message.Reactions.Add(newReaction);
           }
           
                           









            return RedirectToAction("Home", "Home");
        }

        
        
        
    
}