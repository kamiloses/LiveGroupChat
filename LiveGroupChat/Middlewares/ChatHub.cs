using LiveGroupChat.Exceptions;
using LiveGroupChat.Models.Entities;
using LiveGroupChat.Repositories;
using LiveGroupChat.Services;

namespace LiveGroupChat.Middlewares;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly HomeService _homeService;
    private readonly ReactionRepository _reactionRepository;
    private readonly UserRepository _userRepository;
    
    public ChatHub(HomeService homeService, ReactionRepository reactionRepository, UserRepository userRepository)
    {
        _homeService = homeService;
        _reactionRepository = reactionRepository;
        _userRepository = userRepository;
    }

    public async Task SendMessage(string message)
    {
        HttpContext? httpContext = Context.GetHttpContext();
        if (httpContext == null)
            throw new InvalidOperationException("HttpContext is null");

        string? userIdString = httpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString))
            throw new InvalidUserIdException("UserId is missing from session");

        
        int userId = int.Parse(userIdString);

       User user= await _userRepository.FindById(userId);
    

        var messageEntity = new Message
        {
            Text = message,
            UserId = user.Id,
        };

           await _homeService.SendMessage(messageEntity);

        await Clients.All.SendAsync("ReceiveMessage", user.Nickname, message, messageEntity.Id, messageEntity.UserId);
    }


    public async Task GiveEmoji(int messageId, string emoji)
    {
        HttpContext? httpContext = Context.GetHttpContext();
        if (httpContext == null)
            throw new InvalidOperationException("HttpContext is null");

        string? userIdString = httpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString))
            throw new InvalidUserIdException("UserId is missing from session");

        int userId = int.Parse(userIdString);

        var reaction = new Reaction
        {
            MessageId = messageId,
            UserId = userId,
            Emoji = emoji
        };

         await _reactionRepository.SaveReactionAsync(reaction);

        await Clients.All.SendAsync("ReceiveEmoji", messageId, emoji);
    }
}