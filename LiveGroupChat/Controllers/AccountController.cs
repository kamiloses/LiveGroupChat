using LiveGroupChat.Models.Entities;
using LiveGroupChat.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiveGroupChat.Controllers;

public class AccountController : Controller
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [Route("/account/login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [Route("/account/login")]
    public async Task<IActionResult> Login(string nickname)
    {
        await _accountService.LoginAsync(nickname);

        return RedirectToAction("Home", "Home");
    }
}