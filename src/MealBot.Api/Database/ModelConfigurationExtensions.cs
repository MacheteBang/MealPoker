using MealBot.Api.Families.Models;

namespace MealBot.Api.Database;

public static class ModelConfigurationExtensions
{
    public static ModelBuilder ConfigureUser(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId)
            .HasName("PK_Users");

        modelBuilder.Entity<User>()
            .Property(u => u.AuthProvider)
            .HasConversion(
                v => v.ToString(),
                v => (AuthProvider)Enum.Parse(typeof(AuthProvider), v));

        return modelBuilder;
    }
    public static ModelBuilder ConfigureFamily(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Family>()
            .ToTable("Families");

        modelBuilder.Entity<Family>()
            .HasKey(f => f.FamilyId)
            .HasName("PK_Families");
        return modelBuilder;
    }
    public static ModelBuilder ConfigureMeal(this ModelBuilder modelBuilder)
    {
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

        return modelBuilder;
    }
}