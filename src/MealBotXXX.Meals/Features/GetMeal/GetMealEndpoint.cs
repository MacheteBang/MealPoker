using MealBot.Meals;

namespace MealBot.Meals.Features.GetMeal;

public sealed class GetMealEndpoint : MealsEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Globals.BaseRoute + "/{mealId}", async (Guid mealId, ISender sender) =>
        {
            var result = await sender.Send(new GetMealQuery(mealId));

            return result.Match(
                meal => Results.Ok(meal),
                errors => Problem(errors));
        });
    }
}