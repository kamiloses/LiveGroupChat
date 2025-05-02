using System.ComponentModel.DataAnnotations;

namespace LiveGroupChat.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";

    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    
    public List<Message> Messages { get; set; } = new List<Message>();
}