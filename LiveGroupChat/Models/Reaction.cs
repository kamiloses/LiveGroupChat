namespace LiveGroupChat.Models;

public class Reaction
{
    public int Id { get; set; }

    public string Emoji { get; set; } 
    public string UserId { get; set; }

    public int MessageId { get; set; }
    public Message Message { get; set; }
}
