namespace MealBot.Api.Meals.Features.DeleteMeal;

public sealed class DeleteMealEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(GlobalSettings.RoutePaths.Meals + "/{mealId}", async (Guid mealId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteMealCommand(mealId));

            return result.Match(
                success => Results.NoContent(),
                errors => Problem(errors));
        });
    }
}