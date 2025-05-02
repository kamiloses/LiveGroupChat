using LiveGroupChat.Models;
using Microsoft.EntityFrameworkCore;

namespace LiveGroupChat.Data;

public class AppDbContext : DbContext {


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    
}