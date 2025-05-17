using LiveGroupChat.Models.Entities;
using LiveGroupChat.Models.ViewModels;
using LiveGroupChat.Services;
using Microsoft.EntityFrameworkCore;

namespace LiveGroupChat.Middlewares;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub {
    
    private readonly HomeService _homeService;
    private readonly AppDbContext _context;

    public ChatHub(HomeService homeService, AppDbContext context)
    {
        _homeService = homeService;
        _context = context;
    }


    // Metoda wywoływana przez klienta w celu wysłania wiadomości
    public async Task SendMessage(string message)
  //  public async Task SendMessage(string message,string username)
    {    
        Console.BackgroundColor = ConsoleColor.Green;
        var httpContext = Context.GetHttpContext();
        var userIdString = httpContext.Session.GetString("UserId");
        
             User user=  await _context.Users.FirstOrDefaultAsync(user => user.Id.Equals(int.Parse(userIdString)));
        
     
        
       MessageViewModel messageView = new MessageViewModel(){Text = message,User =user };
         messageView.User.Nickname=user.Nickname;   
        _homeService.SendMessage(messageView);
        
        
        // Wysyłanie wiadomości do wszystkich połączonych klientów
        await Clients.All.SendAsync("ReceiveMessage",user.Nickname, message);
    }
    public async Task GiveEmoji(int messageId, string emoji)
    {
        Console.WriteLine($"ODEBRAŁEM emoji: {emoji} dla wiadomości ID: {messageId}");
        _homeService.AddEmoji(messageId, emoji);
        await Clients.All.SendAsync("ReceiveEmoji", messageId, emoji);

    }
    
    
}