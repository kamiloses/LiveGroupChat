using LiveGroupChat.Models.Entities;
using LiveGroupChat.Repositories;

namespace LiveGroupChat.Services;

public class HomeService
{
    private readonly MessageRepository _messageRepository;



    public HomeService(
        MessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
     
    }

    public async Task<List<Message>> GetAllMessagesAsync()
    {



        if (await _messageRepository.CountAsync() >= 20)
        {
            await _messageRepository.RemoveAllAsync();
        }

        return await _messageRepository.GetAllWithRelationsAsync();
    }

    public async Task<Message> SendMessage(Message messageEntity)
    {

       await _messageRepository.SaveMessageAsync(messageEntity);
        return messageEntity;
    }



}