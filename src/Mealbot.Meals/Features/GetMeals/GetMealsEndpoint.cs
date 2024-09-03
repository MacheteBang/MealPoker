
namespace Mealbot.Meals.Features.GetMeals;

public sealed class GetMealsEndpoint : IMealsEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Globals.BaseRoute, async (ISender sender) =>
        {
            var result = await sender.Send(new GetMealsQuery());

            return result.Match(
                meals => Results.Ok(meals),
                error => Results.Problem());
        });
    }
}