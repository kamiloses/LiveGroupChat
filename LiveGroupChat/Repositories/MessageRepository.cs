using LiveGroupChat.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiveGroupChat.Repositories;

public class MessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Message> GetAllWithRelations()
    {
        return _context.Messages.Include(m => m.User)
            .Include(m => m.Reactions)
            .ToList();
    }

    public void RemoveAll()
    {
        _context.Messages.RemoveRange(_context.Messages);
        _context.SaveChanges();
    }

    public int Count() => _context.Messages.Count();

    public Message Add(Message message)
    {
        _context.Messages.Add(message);
        _context.SaveChanges();
        return message;
    }
}