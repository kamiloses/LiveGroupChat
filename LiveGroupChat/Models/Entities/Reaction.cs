
using System.ComponentModel.DataAnnotations;

namespace LiveGroupChat.Models.Entities
{
    public class Reaction
    {
        public int Id { get; set; }

        
        public string Emoji { get; set; } = "";
        public int UserId { get; set; }
        public User User { get; set; } 

        public int MessageId { get; set; }
        public Message Message { get; set; }
    }
}

