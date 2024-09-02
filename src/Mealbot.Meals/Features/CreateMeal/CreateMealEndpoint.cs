namespace Mealbot.Meals.Features.CreateMeal;

public sealed class CreateMealEndpoint : IMealsEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(Globals.BaseRoute, async (CreateMealRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.ToCommand());

            return result.Match(
                meal => Results.Created($"{Globals.BaseRoute}/{meal.MealId}", meal),
                error => Results.Problem());
        });
    }
}