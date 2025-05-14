using LiveGroupChat.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relacja między Message a User
        modelBuilder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany() // Bez kolekcji w User
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacja między Reaction a User
        modelBuilder.Entity<Reaction>()
            .HasOne(r => r.user)
            .WithMany() // Bez kolekcji w User
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict); 

        // Relacja między Reaction a Message
        modelBuilder.Entity<Reaction>()
            .HasOne(r => r.Message)
            .WithMany(m => m.Reactions)
            .HasForeignKey(r => r.MessageId)
            .OnDelete(DeleteBehavior.Cascade); // Kaskadowe usuwanie reakcji po usunięciu wiadomości
    }
}