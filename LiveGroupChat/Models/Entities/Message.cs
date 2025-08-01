﻿using System.ComponentModel.DataAnnotations;

namespace LiveGroupChat.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        
        [StringLength(100)]
        public string Text { get; set; } = "";
        public DateTime Created { get; set; }

        public int UserId { get; set; } 
        
        
        public User? User { get; set; } 

        public List<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}