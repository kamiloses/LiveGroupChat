using System.ComponentModel.DataAnnotations.Schema;

namespace LiveGroupChat.Models
{
    public class Message
    {[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public DateTime Created { get; set; }

        public int UserId { get; set; } 
        public User User { get; set; } 

        public List<Reaction> Reactions { get; set; } = new();
    }
}