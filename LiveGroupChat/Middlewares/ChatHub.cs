namespace LiveGroupChat.Middlewares;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    // odpowiednik @MessageMapping("/chat")
    public async Task SendMessage(string message)
    {
        // odpowiednik @SendTo("/topic/messages")
        await Clients.All.SendAsync("ReceiveMessage", "Echo: " + message);
    }
}
