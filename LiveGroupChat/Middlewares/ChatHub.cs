namespace LiveGroupChat.Middlewares;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    // Metoda wywoływana przez klienta w celu wysłania wiadomości
    public async Task SendMessage(string username, string message)
    {
        
        
        // Wysyłanie wiadomości do wszystkich połączonych klientów
        await Clients.All.SendAsync("ReceiveMessage", username, message);
    }
}