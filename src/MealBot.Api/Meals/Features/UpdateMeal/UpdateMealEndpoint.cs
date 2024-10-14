namespace MealBot.Api.Meals.Features.UpdateMeal;

public sealed class UpdateMealEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut($"{GlobalSettings.RoutePaths.Meals}/{{mealId}}", async (Guid mealId, UpdateMealRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.ToCommand(mealId));

            return result.Match(
                meal => Results.Ok(meal.ToResponse()),
                errors => Problem(errors));
        });
    }
}