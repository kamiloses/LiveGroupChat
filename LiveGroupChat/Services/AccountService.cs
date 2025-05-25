using LiveGroupChat.Models.Entities;
using LiveGroupChat.Repositories;
using Microsoft.AspNetCore.Http;

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

    public async Task<User> LoginAsync(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
        {
            throw new ArgumentException("Nickname cannot be empty.", nameof(nickname));
        }

        var user = new User
        {
            Nickname = nickname + Random.Shared.Next(0, 100)
        };

        var addedUser = await _userRepository.AddAsync(user);

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HTTP context is not available.");
        }

        httpContext.Session.SetString("UserId", addedUser.Id.ToString());

        return addedUser;
    }
}