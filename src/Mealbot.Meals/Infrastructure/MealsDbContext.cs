using Microsoft.EntityFrameworkCore;

namespace MealBot.Meals.Infrastructure;

public class MealsDbContext(DbContextOptions<MealsDbContext> options) : DbContext(options)
{
    public DbSet<Meal> Meals { get; set; }
}
