namespace MealBot.Meals.Features.DeleteMeal;

public sealed class DeleteMealEndpoint : MealsEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(Globals.BaseRoute + "/{mealId}", async (Guid mealId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteMealCommand(mealId));

            return result.Match(
                success => Results.NoContent(),
                error => Results.Problem());
        });
    }
}