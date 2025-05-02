namespace LiveGroupChat.Models;

public class Reaction
{
    public int Id { get; set; }

    public string Emoji { get; set; } 
    public string UserId { get; set; }
    public int Number { get; set; }

    public int MessageId { get; set; }
    public Message Message { get; set; }
    
    
    public override string ToString()
    {
        return $"Reaction [Id={Id}, Emoji={Emoji}, UserId={UserId}, Number={Number}, MessageId={MessageId}]";
    }
    
}
