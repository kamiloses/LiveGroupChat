using LiveGroupChat.Models;
using LiveGroupChat.Services;
using LiveGroupChat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiveGroupChat.Controllers;

public class HomeController : Controller
{
    private static readonly Dictionary<string, List<MessageViewModel>> messages =
        new Dictionary<string, List<MessageViewModel>>();

    private User user = new User();
    private readonly HomeService _homeService;

    public HomeController(HomeService homeService)
    {
        _homeService = homeService;
    }


    [Route("/home")]
    public ActionResult Home()
    {
       List<Message> messages =_homeService.getAllMessages();
      List<MessageViewModel> mappedMessages= messages.Select(message => new MessageViewModel() {Id = message.Id,Created = DateTime.Now,Text = message.Text}).ToList();
        return View(mappedMessages);

    
    }

    [HttpPost]
    [Route("/home")]
    public ActionResult WriteMessage(MessageViewModel message)
    {
        _homeService.SendMessage(message);
        return RedirectToAction("Home", "Home");
    }

    [HttpPost]
    [Route("/home/emoji")]
    public ActionResult AddEmoji(int id, string reaction)
    {
        _homeService.AddEmoji(id, reaction);

        return RedirectToAction("Home", "Home");
    }
}