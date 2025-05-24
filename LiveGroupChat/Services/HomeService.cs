using LiveGroupChat.Models.Entities;
using LiveGroupChat.Repositories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace LiveGroupChat.Services;
    public class HomeService
    {
        private readonly MessageRepository _messageRepository;
        private readonly UserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeService(MessageRepository messageRepository, UserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Message> GetAllMessages()
        {
            int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));

            if (_userRepository.GetById(userId) == null)
            {
                var user = new User() { Id = userId, Nickname = "HomeService" };
                _userRepository.Add(user);
            }

            if (_messageRepository.Count() >= 6)
            {
                _messageRepository.RemoveAll();
            }

            return _messageRepository.GetAllWithRelations();
        }
        
    }
