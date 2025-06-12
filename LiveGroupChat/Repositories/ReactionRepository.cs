using LiveGroupChat.Models.Entities;

namespace LiveGroupChat.Repositories;

public class ReactionRepository
{
    private readonly AppDbContext _context;

    public ReactionRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<Reaction> SaveReactionAsync(Reaction reaction)
    {
        _context.Reactions.Add(reaction);
        await _context.SaveChangesAsync();
        return reaction;
    }
}