using LiveGroupChat.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Reaction> Reactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relacja między Message a User
        modelBuilder.Entity<Message>()
            .HasOne(m => m.ApplicationUser)
            .WithMany() // Bez kolekcji w User
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Brak kaskadowego usuwania

        // Relacja między Reaction a User
        modelBuilder.Entity<Reaction>()
            .HasOne(r => r.ApplicationUser)
            .WithMany() // Bez kolekcji w User
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Brak kaskadowego usuwania

        // Relacja między Reaction a Message
        modelBuilder.Entity<Reaction>()
            .HasOne(r => r.Message)
            .WithMany(m => m.Reactions)
            .HasForeignKey(r => r.MessageId)
            .OnDelete(DeleteBehavior.Cascade); // Kaskadowe usuwanie reakcji po usunięciu wiadomości
    }
}