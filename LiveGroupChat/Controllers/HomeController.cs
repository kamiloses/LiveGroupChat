using LiveGroupChat.Models.ViewModels;
using LiveGroupChat.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using LiveGroupChat.Models.Entities;

namespace LiveGroupChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _homeService;

        public HomeController(HomeService homeService)
        {
            _homeService = homeService;
        }

        [Route("/home")]
        public IActionResult Home()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return Redirect("/account/login");
            }

            var messages = _homeService.GetAllMessages();

            var mappedMessages = messages.Select(message => new MessageViewModel()
            {
                Id = message.Id,
                User = message.User,
                Created = DateTime.Now, 
                Text = message.Text,
                Reactions = message.Reactions?.ToList() ?? new List<Reaction>()
            }).ToList();

            return View(mappedMessages);
        }
    }
}