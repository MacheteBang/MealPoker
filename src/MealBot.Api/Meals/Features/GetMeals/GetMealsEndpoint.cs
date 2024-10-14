namespace MealBot.Api.Meals.Features.GetMeals;

public sealed class GetMealsEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(GlobalSettings.RoutePaths.Meals, async (ISender sender) =>
        {
            var result = await sender.Send(new GetMealsQuery());

            return result.Match(
                meals => Results.Ok(meals.Select(meal => meal.ToResponse())),
                errors => Problem(errors));
        });
    }
}