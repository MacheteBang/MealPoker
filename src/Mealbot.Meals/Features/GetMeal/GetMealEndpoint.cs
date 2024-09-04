using Mealbot.Meals;

namespace MealBot.Meals.Features.GetMeal;

public sealed class GetMealEndpoint : IMealsEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Globals.BaseRoute + "/{mealId}", async (Guid mealId, ISender sender) =>
        {
            var result = await sender.Send(new GetMealQuery(mealId));

            return result.Match(
                meal => Results.Ok(meal),
                error => Results.Problem());
        });
    }
}