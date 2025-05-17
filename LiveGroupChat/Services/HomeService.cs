using LiveGroupChat.Models.Entities;
using LiveGroupChat.Models.ViewModels;

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
            User user = new User() { Id = userId };
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("WYKONUJE");
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        if (_context.Messages.Count() > 6)
        {
            _context.Messages.RemoveRange(_context.Messages);
            _context.SaveChanges();
        }

        return _context.Messages.ToList();
    }

    public void SendMessage(MessageViewModel message)
    {
        int userId = message.User.Id;
        var user = _context.Users.First(u => u.Id == userId);
        Random random = new Random();

        // Tworzenie wiadomości
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

        // 1. Tworzenie użytkowników na potrzeby testów
        User user1 = new User() { Id = userId };
        User user2 = new User() { Id = userId };
        List<User> users = new List<User>() { user1, user2 };
        Random random = new Random();
        User randomUser = users[random.Next(users.Count)];

        // 2. Tworzenie obiektu reakcji
        int reactionId = new Random().Next();
        Reaction reaction = new Reaction()
        {
            Id = reactionId,
            Emoji = emoji,
            MessageId = messageId,
            UserId = randomUser.Id,
            user = randomUser
        };
        
        //3 znajduje obiekt wiadomosci z bazy danych któremu dałem reakcje
        Console.BackgroundColor= ConsoleColor.Green;
        Console.WriteLine(messageId + " UserId "+randomUser.Id);
        Message message = _context.Messages.Single(message =>message.Id == messageId);//message.UserId == randomUser.Id &&

        //4 czy ta oceniłem wczesniej wiadomosc reakcją
        bool wasEvaluated= message.Reactions.Any(reaction => reaction.UserId == userId);
        if (!wasEvaluated) {
            message.Reactions.Add(reaction); }
        else {
            //todo transactional
            var reactionToRemove = message.Reactions.Single(reaction => reaction.UserId == userId);
            message.Reactions.Remove(reactionToRemove);
            message.Reactions.Add(reaction);
        }
        }
}