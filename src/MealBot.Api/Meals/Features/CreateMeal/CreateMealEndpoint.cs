namespace MealBot.Api.Meals.Features.CreateMeal;

public sealed class CreateMealEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(GlobalSettings.RoutePaths.Meals, async (HttpContext context, CreateMealRequest request, ISender sender) =>
        {
            if (!Guid.TryParse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out Guid userId))
            {
                return Problem(Identity.Errors.SubMissingFromToken());
            }

            var result = await sender.Send(
                new CreateMealCommand(
                    userId,
                    request.Name,
                    request.Description,
                    request.MealParts)
            );

            return result.Match(
                meal => Results.Created($"{GlobalSettings.RoutePaths.Meals}/{meal.MealId}", meal.ToResponse()),
                errors => Problem(errors));
        })
        .RequireAuthorization();
    }
}