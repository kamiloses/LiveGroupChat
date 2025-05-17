using LiveGroupChat.Models.Entities;

namespace LiveGroupChat.Models.ViewModels;

public class MessageViewModel
{
    
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime Created { get; set; }
    public User? User { get; set; }
    
    public List<Reaction> Reactions { get; set; } =new();

    
    }

