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
        List<Message> messages = _homeService.getAllMessages();
        List<MessageViewModel> mappedMessages = messages.Select(message => new MessageViewModel()
            { Id = message.Id, Created = DateTime.Now, Text = message.Text }).ToList();
        
        foreach (var messageViewModel in mappedMessages)
        {
            Console.WriteLine(messageViewModel.Id);
        }
        return View(mappedMessages); }
}