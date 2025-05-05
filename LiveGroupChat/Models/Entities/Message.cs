using System.ComponentModel.DataAnnotations.Schema;
using LiveGroupChat.Models.IdentityEntities;

namespace LiveGroupChat.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public DateTime Created { get; set; }

        public int UserId { get; set; } 
        public ApplicationUser ApplicationUser { get; set; } 

        public List<Reaction> Reactions { get; set; } = new();
    }
}