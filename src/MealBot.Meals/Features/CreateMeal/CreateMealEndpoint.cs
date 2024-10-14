namespace MealBot.Meals.Features.CreateMeal;

public sealed class CreateMealEndpoint : MealsEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(Globals.BaseRoute, async (CreateMealRequest request, ISender sender) =>
        {
            var result = await sender.Send(
                new CreateMealCommand(request.Name, request.Description, request.MealParts)
            );

            return result.Match(
                meal => Results.Created($"{Globals.BaseRoute}/{meal.MealId}", meal.ToResponse()),
                errors => Problem(errors));
        });
    }
}