﻿using LiveGroupChat.Models.Entities;
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

    public List<Message> GetAllMessages()
    {
        int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
        if (!_context.Users.Any(user => user.Id == userId))
        {
            User user = new User() { Id = userId, Nickname = "HomeService" };
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        if (_context.Messages.Count() >= 6)
        {
            _context.Messages.RemoveRange(_context.Messages);
            _context.SaveChanges();
        }

        List<Message> messages = _context.Messages.Include(user => user.User)
            .Include(m => m.Reactions).ToList();
        Console.Write(messages);

        return messages;
    }

  
    
}