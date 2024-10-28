namespace MealBot.Api.Meals.Features.UpdateMeal;

public sealed class UpdateMealEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut($"{GlobalSettings.RoutePaths.Meals}/{{mealId}}", async (
            HttpContext context,
            Guid mealId,
            UpdateMealRequest request,
            ISender sender) =>
        {
            if (!Guid.TryParse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out Guid userId))
            {
                return Problem(Auth.Errors.SubMissingFromToken());
            }

            var result = await sender.Send(new UpdateMealCommand(
                userId,
                mealId,
                request.Name,
                request.Description,
                request.MealParts));

            return result.Match(
                meal => Results.Ok(meal.ToResponse()),
                errors => Problem(errors));
        })
        .RequireAuthorization();
    }
}