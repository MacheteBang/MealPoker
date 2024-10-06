using Microsoft.EntityFrameworkCore;

namespace MealBot.Auth.Database;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.EmailAddress)
            .HasName("PK_Users_EmailAddress");
    }
}
