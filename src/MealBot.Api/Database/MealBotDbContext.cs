using Microsoft.EntityFrameworkCore;

namespace MealBot.Api.Database;

internal sealed class MealBotDbContext(DbContextOptions<MealBotDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Meal> Meals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId)
            .HasName("PK_Users");
        modelBuilder.Entity<User>()
            .Property(u => u.AuthProvider)
            .HasConversion(
                v => v.ToString(),
                v => (AuthProvider)Enum.Parse(typeof(AuthProvider), v));

        modelBuilder.Entity<Meal>()
            .HasKey(m => m.MealId)
            .HasName("PK_Meals");
        modelBuilder.Entity<Meal>()
            .OwnsMany(m => m.MealParts, a =>
            {
                a.ToTable("MealParts");
                a.WithOwner().HasForeignKey("MealId");
                a.Property<int>("MealPartId");
                a.HasKey("MealPartId");
            });
    }
}
