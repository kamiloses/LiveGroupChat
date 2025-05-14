using LiveGroupChat.Models;
using LiveGroupChat.Models.Entities;

namespace LiveGroupChat.Models.ViewModels;

public class ReactionViewModel
{
 
        public int Id { get; set; }

        public string Emoji { get; set; } 
        public int number { get; set; }
        public string UserId { get; set; }

        public int MessageId { get; set; }
        public Message Message { get; set; }
    

}