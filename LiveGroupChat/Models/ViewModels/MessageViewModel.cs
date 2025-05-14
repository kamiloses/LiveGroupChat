using LiveGroupChat.Models.Entities;

namespace LiveGroupChat.Models.ViewModels;

public class MessageViewModel
{
    
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime Created { get; set; }
    public UserViewModel? User { get; set; }
    
    public List<Reaction> Reactions { get; set; } =new();

    
    
    public override string ToString()
    {
        string? userInfo = null;

        if (User != null)
        {
            List<string> userParts = new();

            if (!string.IsNullOrWhiteSpace(User.Id))
                userParts.Add($"Id: {User.Id}");

            if (!string.IsNullOrWhiteSpace(User.FirstName))
                userParts.Add($"FirstName: {User.FirstName}");

            if (!string.IsNullOrWhiteSpace(User.LastName))
                userParts.Add($"LastName: {User.LastName}");

            if (userParts.Count > 0)
                userInfo = string.Join(", ", userParts);
        }

        string reactionsString = Reactions != null && Reactions.Count > 0
            ? string.Join(Environment.NewLine, Reactions.Select(r => r.ToString()))
            : "Brak";

        return $"Message ID: {Id}, " +
               $"Text: \"{Text}\", " +
               $"Created: {Created}, " +
               $"User: {userInfo}, " +
               $"Reactions:\n{reactionsString}";
    }

}