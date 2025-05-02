using LiveGroupChat.Data;
using LiveGroupChat.Models;
using LiveGroupChat.ViewModels;

namespace LiveGroupChat.Services;

public class HomeService
{
    private readonly AppDbContext _context;


    
    public void SendMessage(MessageViewModel message)
    {

        //1  pobieram jednego użytkownika
        User user1 = new User() { Id = 1, FirstName = "Jan", LastName = "Nowak" };
        User user2 = new User() { Id = 2, FirstName = "Maciej", LastName = "Kowalski" };
        User user3 = new User() { Id = 3, FirstName = "Adam", LastName = "Nowacki" };
        User user4 = new User() { Id = 4, FirstName = "Dawid", LastName = "Szymański" };
        User user5 = new User() { Id = 5, FirstName = "Jakub", LastName = "Lewandowski" };
        List<User> users = new List<User>() { user1, user2, user3, user4, user5 };
        Random random = new Random();
        User randomUser = users[random.Next(users.Count)];



        //2 tworze obiekt message i zapisuje go (sprawdz potem by pusty string nie był wysyłany)
        long randomNumber = random.NextInt64(1, 1000);
        Message messageEntity = new Message() { User = randomUser, Text = message.Text! };

        //message.User = new UserViewModel() { Id = randomNumber.ToString(), FirstName = "Maciej", LastName = "Nowak" };
        _context.Messages.Add(messageEntity);



    }
}