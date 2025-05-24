using LiveGroupChat.Models.Entities;

namespace LiveGroupChat.Repositories;

public class ReactionRepository
{
    private readonly AppDbContext _context;

    public ReactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Reaction? GetByMessageAndUser(int messageId, int userId)
    {
        return _context.Reactions.FirstOrDefault(r => r.MessageId == messageId && r.UserId == userId);
    }

    public Reaction Add(Reaction reaction)
    {
        _context.Reactions.Add(reaction);
        _context.SaveChanges();
        return reaction;
    }
}