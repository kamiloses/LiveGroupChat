﻿using LiveGroupChat.Models.Entities;
using LiveGroupChat.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LiveGroupChat.Services;

public class HomeService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HomeService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public List<Message> getAllMessages()
    {
        int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
        Console.BackgroundColor = ConsoleColor.Red; 
        Console.WriteLine("ID"+userId);
        if (!_context.Users.Any(user => user.Id == userId))
        {
            User user = new User() { Id = userId, Nickname = "HomeService" };
            Console.BackgroundColor = ConsoleColor.Green;
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        if (_context.Messages.Count() >= 6)
        {
            _context.Messages.RemoveRange(_context.Messages);
            _context.SaveChanges();
        }

        List<Message> messages = _context.Messages.Include(user => user.User).ToList();
        Console.Write(messages);

        return messages;
    }

    public void SendMessage(MessageViewModel message)
    {
        int userId = message.User.Id;
        var user = _context.Users.First(u => u.Id == userId);

        var messageEntity = new Message
        {
            Text = message.Text!,
            Created = DateTime.Now,
            UserId = user.Id,
            User = user
        };

        _context.Messages.Add(messageEntity);
        _context.SaveChanges();
    }

    public void AddEmoji(int messageId, string emoji)
    {
        int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));

        var message = _context.Messages
            .Include(m => m.Reactions)
            .FirstOrDefault(m => m.Id == messageId);

        if (message == null) return;

        var existingReaction = message.Reactions.FirstOrDefault(r => r.UserId == userId);

        if (existingReaction != null)
        {
            if (existingReaction.Emoji == emoji)
                return;

            _context.Reactions.Remove(existingReaction);
        }

        var reaction = new Reaction
        {
            Emoji = emoji,
            MessageId = messageId,
            UserId = userId
        };

        _context.Reactions.Add(reaction);
        _context.SaveChanges();
    }

}