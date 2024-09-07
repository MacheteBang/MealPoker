namespace MealBot.Meals.Features.UpdateMeal;

public sealed class UpdateMealEndpoint : MealsEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut($"{Globals.BaseRoute}/{{mealId}}", async (Guid mealId, UpdateMealRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.ToCommand(mealId));

            return result.Match(
                meal => Results.Ok(meal),
                error => Results.Problem());
        });
    }
}