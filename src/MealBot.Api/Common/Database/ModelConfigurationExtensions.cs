namespace MealBot.Api.Common.Database;

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

        modelBuilder.Entity<Family>()
            .HasMany(f => f.User);

        modelBuilder.Entity<Family>()
            .HasIndex(f => f.Code)
            .IsUnique();

        return modelBuilder;
    }
    public static ModelBuilder ConfigureMeal(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meal>()
            .HasKey(m => m.MealId)
            .HasName("PK_Meals");

        modelBuilder.Entity<Meal>()
            .HasOne(m => m.Owner);

        modelBuilder.Entity<Meal>()
            .OwnsMany(m => m.MealParts, a =>
            {
                a.ToTable("MealParts");
                a.WithOwner().HasForeignKey("MealId");
                a.Property<int>("MealPartId");
                a.HasKey("MealPartId");
                a.Property(p => p.Category)
                    .HasConversion(
                        v => v.ToString(),
                        v => (MealPartCategory)Enum.Parse(typeof(MealPartCategory), v));
            });

        return modelBuilder;
    }
    public static ModelBuilder ConfigureUserMealRating(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserMealRating>()
            .HasKey(umr => new { umr.MealId, umr.UserId })
            .HasName("PK_UserMealRatings");

        modelBuilder.Entity<UserMealRating>()
            .HasOne(umr => umr.Meal)
            .WithMany(m => m.Ratings);

        modelBuilder.Entity<UserMealRating>()
            .HasOne(umr => umr.User);

        modelBuilder.Entity<UserMealRating>()
            .Property(u => u.Rating)
            .HasConversion(
                v => v.ToString(),
                v => (MealRating)Enum.Parse(typeof(MealRating), v));

        return modelBuilder;
    }
}