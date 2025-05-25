using LiveGroupChat.Models.Entities;
using LiveGroupChat.Repositories;
using Microsoft.AspNetCore.Http;

namespace LiveGroupChat.Services;

public class HomeService
{
    private readonly MessageRepository _messageRepository;
    private readonly UserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HomeService(
        MessageRepository messageRepository,
        UserRepository userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Message>> GetAllMessagesAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HTTP context is not available.");
        }

        var userIdString = httpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            throw new InvalidOperationException("UserId is not available in session.");
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            user = new User { Id = userId, Nickname = "HomeService" };
            await _userRepository.AddAsync(user);
        }

        if (await _messageRepository.CountAsync() >= 6)
        {
            await _messageRepository.RemoveAllAsync();
        }

        return await _messageRepository.GetAllWithRelationsAsync();
    }
}