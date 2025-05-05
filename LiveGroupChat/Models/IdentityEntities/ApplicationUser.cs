using Microsoft.AspNetCore.Identity;

namespace LiveGroupChat.Models.IdentityEntities;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
