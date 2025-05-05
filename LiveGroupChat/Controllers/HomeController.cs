using LiveGroupChat.Models;
using LiveGroupChat.Services;
using LiveGroupChat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LiveGroupChat.Controllers;

public class HomeController : Controller
{
    private static readonly Dictionary<string, List<MessageViewModel>> messages =
        new Dictionary<string, List<MessageViewModel>>();

    private ApplicationUser _applicationUser = new ApplicationUser();
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
    
    
    
    

//todo websockety złe id wyswietlają wiec to zmienic w bazie danych jest ok wszystko
}