using LiveGroupChat.Exceptions;
using LiveGroupChat.Models.Entities;
using LiveGroupChat.Models.ViewModels;
using LiveGroupChat.Services;
using Microsoft.EntityFrameworkCore;

namespace LiveGroupChat.Middlewares;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub {
    
    private readonly AppDbContext _context;

    public ChatHub(AppDbContext context)
    {
        _context = context;
    }

    public async Task SendMessage(string message)
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            throw new InvalidOperationException("HttpContext is null");

        var userIdString = httpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString))
            throw new InvalidUserIdException("UserId is missing from session");

        if (!int.TryParse(userIdString, out var userId))
            throw new InvalidUserIdException("UserId from session is invalid");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new InvalidUserIdException("User not found");

        var messageEntity = new Message
        {
            Text = message,
            UserId = user.Id,
        };

        _context.Messages.Add(messageEntity);
        await _context.SaveChangesAsync();

        await Clients.All.SendAsync("ReceiveMessage", user.Nickname, message, messageEntity.Id);
    }


    public async Task GiveEmoji(int messageId, string emoji)
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            throw new InvalidOperationException("HttpContext is null");

        var userIdString = httpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString))
            throw new InvalidUserIdException("UserId is missing from session");

        if (!int.TryParse(userIdString, out var userId))
            throw new InvalidUserIdException("UserId from session is invalid");

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