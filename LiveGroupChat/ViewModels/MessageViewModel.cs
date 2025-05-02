using LiveGroupChat.Models;

namespace LiveGroupChat.ViewModels;

public class MessageViewModel
{
    
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime Created { get; set; }
    public UserViewModel? User { get; set; }
    
    public List<Reaction> Reactions { get; set; } =new();

}