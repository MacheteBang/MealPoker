namespace MealBot.Api.Common.Database;

internal sealed class MealBotDbContext(DbContextOptions<MealBotDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Family> Families { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<UserMealRating> UserMealRatings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder
        .ConfigureUser()
        .ConfigureFamily()
        .ConfigureMeal()
        .ConfigureUserMealRating();
}