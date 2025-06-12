using LiveGroupChat.Exceptions;
using LiveGroupChat.Models.Entities;
using LiveGroupChat.Services;
using Microsoft.EntityFrameworkCore;

namespace LiveGroupChat.Middlewares;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly AppDbContext _context;
    private readonly HomeService _homeService;

    public ChatHub(AppDbContext context, HomeService homeService)
    {
        _context = context;
        _homeService = homeService;
    }

    public async Task SendMessage(string message)
    {
        HttpContext? httpContext = Context.GetHttpContext();
        if (httpContext == null)
            throw new InvalidOperationException("HttpContext is null");

        string? userIdString = httpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString))
            throw new InvalidUserIdException("UserId is missing from session");

        
        int userId = int.Parse(userIdString);
        

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new InvalidUserIdException("User not found");

        var messageEntity = new Message
        {
            Text = message,
            UserId = user.Id,
        };

           await _homeService.SendMessage(messageEntity);

        await Clients.All.SendAsync("ReceiveMessage", user.Nickname, message, messageEntity.Id, messageEntity.UserId);
    }


    public async Task GiveEmoji(int messageId, string emoji)
    {
        HttpContext? httpContext = Context.GetHttpContext();
        if (httpContext == null)
            throw new InvalidOperationException("HttpContext is null");

        string? userIdString = httpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString))
            throw new InvalidUserIdException("UserId is missing from session");

        int userId = int.Parse(userIdString);

        var reaction = new Reaction
        {
            MessageId = messageId,
            UserId = userId,
            Emoji = emoji
        };

        _context.Reactions.Add(reaction);
        await _context.SaveChangesAsync();

        await Clients.All.SendAsync("ReceiveEmoji", messageId, emoji);
    }
}