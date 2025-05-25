using LiveGroupChat.Models.Entities;
using LiveGroupChat.Models.ViewModels;
using LiveGroupChat.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiveGroupChat.Controllers;

public class HomeController : Controller
{
    private readonly HomeService _homeService;

    public HomeController(HomeService homeService)
    {
        _homeService = homeService;
    }

    [Route("/home")]
    public async Task<IActionResult> Home()
    {
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString))
        {
            return Redirect("/account/login");
        }

        var messages = await _homeService.GetAllMessagesAsync();

        var mappedMessages = messages.Select(message => new MessageViewModel
        {
            Id = message.Id,
            Text = message.Text,
            User = message.User,
            Reactions = message.Reactions?.ToList() ?? new()
        }).ToList();

        return View(mappedMessages);
    }
}