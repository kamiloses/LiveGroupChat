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

    public async Task<List<Message>> GetAllWithRelationsAsync()
    {
        return await _context.Messages
            .Include(m => m.User)
            .Include(m => m.Reactions)
            .ToListAsync();
    }


    public async Task RemoveAllAsync()
    {
        _context.Messages.RemoveRange(_context.Messages);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Messages.CountAsync();
    }

    public async Task<Message> SaveMessageAsync(Message message)
    {
         _context.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }
}