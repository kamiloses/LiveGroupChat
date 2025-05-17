using LiveGroupChat.Models.Entities;
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

        if (!_context.Users.Any(user => user.Id == userId))
        {
            User user = new User() { Id = userId, Nickname = "HomeService" };
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("EXECUTING");
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        if (_context.Messages.Count() > 6)
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

        User user1 = new User() { Id = userId };
        User user2 = new User() { Id = userId };
        List<User> users = new List<User>() { user1, user2 };
        Random random = new Random();
        User randomUser = users[random.Next(users.Count)];

        int reactionId = new Random().Next();
        Reaction reaction = new Reaction()
        {
            Id = reactionId,
            Emoji = emoji,
            MessageId = messageId,
            UserId = randomUser.Id,
            user = randomUser
        };

        Console.BackgroundColor = ConsoleColor.Green;
        Console.WriteLine(messageId + " UserId " + randomUser.Id);

        Message message = _context.Messages.Single(message => message.Id == messageId);

        bool wasEvaluated = message.Reactions.Any(r => r.UserId == userId);
        if (!wasEvaluated)
        {
            message.Reactions.Add(reaction);
        }
        else
        {
            var reactionToRemove = message.Reactions.Single(r => r.UserId == userId);
            message.Reactions.Remove(reactionToRemove);
            message.Reactions.Add(reaction);
        }
    }
}
