
using LiveGroupChat.Models.Entities;
using LiveGroupChat.Repositories;

namespace LiveGroupChat.Services;

public class AccountService
{
    private readonly UserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(UserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public User Login(string nickname)
    {
        var user = new User { Nickname = nickname + Random.Shared.Next(0, 100) };
        var addedUser = _userRepository.Add(user);

        _httpContextAccessor.HttpContext.Session.SetString("UserId", addedUser.Id.ToString());

        return addedUser;
    }
}

