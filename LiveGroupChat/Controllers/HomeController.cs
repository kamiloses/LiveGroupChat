    using LiveGroupChat.Models;
    using LiveGroupChat.Models.Entities;
    using LiveGroupChat.Services;
    using LiveGroupChat.Models.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    namespace LiveGroupChat.Controllers;

    public class HomeController : Controller
    {
        private static readonly Dictionary<string, List<MessageViewModel>> messages =
            new Dictionary<string, List<MessageViewModel>>();

        private readonly HomeService _homeService;
        

        public HomeController(HomeService homeService)
        {
            _homeService = homeService;
        }


        [Route("/home")]
        public ActionResult Home()
        {
            
            var userIdString = HttpContext.Session.GetString("UserId");
            ViewData["UserId"] = userIdString;
            if (string.IsNullOrEmpty(userIdString))
            {
                return Redirect("/account/login");
            }
            
            
            
            List<Message> messages = _homeService.GetAllMessages();
            List<MessageViewModel> mappedMessages = messages.Select(message => new MessageViewModel()
            {
                Id = message.Id,User = message.User, Created = DateTime.Now, Text = message.Text,
                Reactions = message.Reactions?.ToList() ?? new List<Reaction>()
                
            }).ToList();
            
                     
            
            foreach (var messageViewModel in mappedMessages)
            {
            }
            return View(mappedMessages); }
    }//TODO OD KONTROLERA W GÓRE