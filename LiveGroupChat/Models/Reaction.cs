using System.ComponentModel.DataAnnotations.Schema;

namespace LiveGroupChat.Models
{
    public class Reaction
    {[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Emoji { get; set; } = "";
        public int UserId { get; set; }
        public User User { get; set; } 

        public int MessageId { get; set; }
        public Message Message { get; set; }
    }
}

