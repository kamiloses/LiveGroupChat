using System.ComponentModel.DataAnnotations.Schema;
using LiveGroupChat.Models.IdentityEntities;

namespace LiveGroupChat.Models
{
    public class Reaction
    {
        public int Id { get; set; }

        public string Emoji { get; set; } = "";
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } 

        public int MessageId { get; set; }
        public Message Message { get; set; }
    }
}

