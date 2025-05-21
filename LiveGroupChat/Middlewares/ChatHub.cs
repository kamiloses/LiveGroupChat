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
        var userIdString = httpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userIdString)) return;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userIdString));
        if (user == null) return;

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
        var userIdString = httpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            return;

        var existingReaction = await _context.Reactions
            .FirstOrDefaultAsync(r => r.MessageId == messageId && r.UserId == userId);

        if (existingReaction != null)
        {
            return;
        }

        var reaction = new Reaction
        {
            MessageId = messageId,
            UserId = userId,
            Emoji = emoji,
        };

        _context.Reactions.Add(reaction);
        await _context.SaveChangesAsync();

        await Clients.All.SendAsync("ReceiveEmoji", messageId, emoji);
    }

    
}