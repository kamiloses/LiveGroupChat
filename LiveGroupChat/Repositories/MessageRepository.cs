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

    public async Task<Message> AddAsync(Message message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
        return message;
    }
}