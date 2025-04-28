namespace LiveGroupChat.Models;

public class Message
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime Created { get; set; }
    public int? UserId { get; set; }
    
    
    
    
}