namespace MealBot.Api.Meals.Features.CreateMeal;

public sealed class CreateMealEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(GlobalSettings.RoutePaths.Meals, async (CreateMealRequest request, ISender sender) =>
        {
            var result = await sender.Send(
                new CreateMealCommand(request.Name, request.Description, request.MealParts)
            );

            return result.Match(
                meal => Results.Created($"{GlobalSettings.RoutePaths.Meals}/{meal.MealId}", meal.ToResponse()),
                errors => Problem(errors));
        });
    }
}