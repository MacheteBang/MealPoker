namespace MealBot.Api.Meals.Features.RateMeal;

public sealed class RateMealEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{GlobalSettings.RoutePaths.Meals}/{{mealId}}/ratings/{{rating}}", async (
            HttpContext context,
            Guid mealId,
            string rating,
            ISender sender) =>
        {
            if (!Guid.TryParse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out Guid userId))
            {
                return Problem(Identity.Errors.SubMissingFromToken());
            }

            if (!Enum.TryParse<MealRating>(rating, out var mealRating))
            {
                return Problem(Errors.InvalidRating(rating));
            }

            var result = await sender.Send(new RateMealCommand(
                userId,
                mealId,
                mealRating));

            return result.Match(
                _ => Results.Ok(),
                errors => Problem(errors));
        })
        .RequireAuthorization();
    }
}