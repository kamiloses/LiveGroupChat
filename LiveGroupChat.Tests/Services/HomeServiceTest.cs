using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LiveGroupChat.Models.Entities;
using LiveGroupChat.Repositories;
using LiveGroupChat.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LiveGroupChat.Tests.Services
{
    public class HomeServiceTests
    {
        private readonly MessageRepository _messageRepository;
        private readonly AppDbContext _context;
        private readonly HomeService _homeService;

        public HomeServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var options = optionsBuilder.Options;

            
            _context = new AppDbContext(options);
            _messageRepository = new MessageRepository(_context);
            _homeService = new HomeService(_messageRepository);
            
            

            User user = new User() { Id = 1, Nickname = "kamiloses" };
            _context.Users.Add(user);
            _context.SaveChanges();

            Message message = new Message()
            {
                Text = "text",
                UserId = user.Id,
                User = user,
                Reactions = new List<Reaction>
                {
                    new Reaction { Emoji = "😮" },
                    new Reaction { Emoji = "❤️" }
                }
            };
            _context.Messages.Add(message);
            _context.SaveChanges();
        }

        [Fact] // todo  ChatHub ogarnąć,HomeController
        public async Task Should_GetAllMessagesAsync()
        {
            List<Message> messages = await _homeService.GetAllMessagesAsync();
            Assert.True(messages.Count == 1);
            Assert.NotNull(messages[0].User);                            
            Assert.Equal("kamiloses", messages[0].User.Nickname);       
            Assert.Equal(2, messages[0].Reactions.Count);                
            Assert.Contains(messages[0].Reactions, r => r.Emoji == "😮");
            Assert.Contains(messages[0].Reactions, r => r.Emoji == "❤️");
            
        }

   
    }
}
