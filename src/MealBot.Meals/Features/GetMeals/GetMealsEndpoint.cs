
namespace MealBot.Meals.Features.GetMeals;

public sealed class GetMealsEndpoint : MealsEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Globals.BaseRoute, async (ISender sender) =>
        {
            var result = await sender.Send(new GetMealsQuery());

            return result.Match(
                meals => Results.Ok(meals.Select(meal => meal.ToResponse())),
                errors => Problem(errors));
        });
    }
}