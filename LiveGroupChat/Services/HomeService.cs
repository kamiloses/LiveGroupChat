using LiveGroupChat.Data;
using LiveGroupChat.Models;
using LiveGroupChat.ViewModels;

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



    public List<Message> getAllMessages() {
        
        int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId").Substring(0, 4));

        if (_context.Users.Any(user => user.Id == userId)){
            User user = new User() { Id = userId, FirstName = "Jan", LastName = "Nowak" };
            _context.Users.Add(user); }

        
        // if (_context.Messages.Count() > 10)
        // {
        //     _context.Messages.RemoveRange(_context.Messages);
        //     _context.SaveChanges();
        // }
        
         
        
        return _context.Messages;
            
        
        
        
    }
    
    
    
    
    
    
    
    
    
    public void SendMessage(MessageViewModel message)
    {

        //1  pobieram jednego użytkownika
        int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId").Substring(0, 4));
        User user1 = new User() { Id = userId, FirstName = "Jan", LastName = "Nowak" };
        User user2 = new User() { Id = userId, FirstName = "Maciej", LastName = "Kowalski" };
        User user3 = new User() { Id = userId, FirstName = "Jakub", LastName = "Lewandowski" };
        List<User> users = new List<User>() { user1, user2, user3 };
        Random random = new Random();
        User randomUser = users[random.Next(users.Count)];



        //2 tworze obiekt message i zapisuje go (sprawdz potem by pusty string nie był wysyłany)
        int randomNumber = random.Next(1, 1000);
        Message messageEntity = new Message() {Id =randomNumber, User = randomUser, Text = message.Text! };

        _context.Messages.Add(messageEntity);



    }
    
    public void AddEmoji(int messageId, string emoji)
    {

        int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId").Substring(0, 4));

        
        //1  pobieram jednego użytkownika
        User user1 = new User() { Id = userId, FirstName = "Jan", LastName = "Nowak" };
        User user2 = new User() { Id = userId, FirstName = "Maciej", LastName = "Kowalski" };
        List<User> users = new List<User>() { user1, user2};
        Random random = new Random();
        User randomUser = users[random.Next(users.Count)];
        
        
        
        //2 tworze obiekt reakcja na post
        int reactionId = new Random().Next();
        Reaction reaction = new Reaction(){Id = reactionId,Emoji = emoji,MessageId = messageId,UserId = randomUser.Id,User = randomUser };

        
        //3 znajduje obiekt wiadomosci z bazy danych któremu dałem reakcje
        Message message = _context.Messages.Single(message => message.UserId == userId && message.Id == messageId);

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