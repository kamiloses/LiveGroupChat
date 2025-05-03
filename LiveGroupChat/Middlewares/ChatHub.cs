using LiveGroupChat.Services;
using LiveGroupChat.ViewModels;

namespace LiveGroupChat.Middlewares;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub {
    
    private readonly HomeService _homeService;

    public ChatHub(HomeService homeService)
    {
        _homeService = homeService;
    }


    // Metoda wywoływana przez klienta w celu wysłania wiadomości
    public async Task SendMessage(string username, string message)
    {
       MessageViewModel messageView = new MessageViewModel(){Text = message};
        
        _homeService.SendMessage(messageView);
        
        // Wysyłanie wiadomości do wszystkich połączonych klientów
        await Clients.All.SendAsync("ReceiveMessage", username, message);
    }
    public async Task GiveEmoji(int messageId, string emoji)
    {
        Console.WriteLine($"ODEBRAŁEM emoji: {emoji} dla wiadomości ID: {messageId}");
        _homeService.AddEmoji(messageId, emoji);
        await Clients.All.SendAsync("ReceiveEmoji", messageId, emoji);

    }
    
    
}