namespace MealBot.Api.Families.Services;

internal interface IFamilyService
{
    Task AddFamilyAsync(Family family);
}

internal class FamilyService(MealBotDbContext mealBotDbContext) : IFamilyService
{
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task AddFamilyAsync(Family family)
    {
        await _mealBotDbContext.Families.AddAsync(family);
        await _mealBotDbContext.SaveChangesAsync();
    }
}