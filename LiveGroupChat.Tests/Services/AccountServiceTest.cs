using System.Threading.Tasks;
using JetBrains.Annotations;
using LiveGroupChat.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LiveGroupChat.Tests.Services;

[TestSubject(typeof(AccountService))]
public class AccountServiceTest
{

    private readonly AccountService _accountService;
     private readonly AppDbContext _context;
     private readonly IHttpContextAccessor _httpContextAccessor;
    public AccountServiceTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
             .UseInMemoryDatabase("TestDatabase").Options;


        _context=new AppDbContext(options);
        _httpContextAccessor = new HttpContextAccessor();
        _accountService = new AccountService(_context,_httpContextAccessor);
    }

    [Fact]
    public async Task should_check_loginAsync()
    {
        var nickname = "testuser";
        var createdUser = await _accountService.LoginAsync(nickname);
        var userFromDb = await _context.Users.FindAsync(createdUser.Id);
        
        Assert.NotNull(userFromDb);
        Assert.Equal(userFromDb.Id, createdUser.Id);
        
    }
        
}