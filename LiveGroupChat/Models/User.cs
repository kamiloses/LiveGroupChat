using System.ComponentModel.DataAnnotations.Schema;

namespace LiveGroupChat.Models
{
    public class User
    {[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}