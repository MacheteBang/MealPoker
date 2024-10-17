using Microsoft.EntityFrameworkCore;

namespace MealBot.Api.Database;

internal sealed class MealBotDbContext(DbContextOptions<MealBotDbContext> options) : DbContext(options)
{
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Meal> Meals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>()
            .HasKey(u => new { u.UserId, u.Subject })
            .HasName("PK_Permissions_UserId_Subject");

        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId)
            .HasName("PK_Users_UserId");

        modelBuilder.Entity<Meal>()
            .HasKey(m => m.MealId)
            .HasName("PK_Meals_MealId");
        modelBuilder.Entity<Meal>()
            .OwnsMany(m => m.MealParts, a =>
            {
                a.WithOwner().HasForeignKey("MealId");
                a.Property<int>("Id");
                a.HasKey("Id");
            });
    }
}
