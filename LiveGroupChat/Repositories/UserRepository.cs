using LiveGroupChat.Exceptions;
using LiveGroupChat.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiveGroupChat.Repositories;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> AddAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> FindById(int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new InvalidUserIdException("User not found");
        
        return user;
    }
    
    
    
}