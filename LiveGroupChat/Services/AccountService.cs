using LiveGroupChat.Models.Entities;
namespace LiveGroupChat.Services;

public class AccountService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<User> LoginAsync(string nickname) {
        
        var user = new User { Nickname = nickname };
        _context.Users.Add(user);
       await _context.SaveChangesAsync();

        _httpContextAccessor.HttpContext?.Session.SetString("UserId", user.Id.ToString());
        return user;
    }
}