using LiveGroupChat.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiveGroupChat.Repositories;

public class ReactionRepository
{
    private readonly AppDbContext _context;

    public ReactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Reaction?> GetByMessageAndUserAsync(int messageId, int userId)
    {
        return await _context.Reactions
            .FirstOrDefaultAsync(r => r.MessageId == messageId && r.UserId == userId);
    }

    public async Task<Reaction> AddAsync(Reaction reaction)
    {
        if (reaction == null) throw new ArgumentNullException(nameof(reaction));

        await _context.Reactions.AddAsync(reaction);
        await _context.SaveChangesAsync();
        return reaction;
    }
}