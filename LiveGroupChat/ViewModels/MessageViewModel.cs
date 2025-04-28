namespace LiveGroupChat.ViewModels;

public class MessageViewModel
{
    public string? Text { get; set; }
    public DateTime Created { get; set; }
    public string? UserFirstName { get; set; }
    public string? UserLastName { get; set; }
    public int? UserImage { get; set; }

}