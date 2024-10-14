namespace MealBot.Api.Meals.Features.GetMealPartCategories;

public sealed class GetMealPartCategoriesEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(GlobalSettings.RoutePaths.MealPartCategories, async (ISender sender) =>
        {
            var result = await sender.Send(new GetMealPartCategoriesQuery());

            return result.Match(
                categories => Results.Ok(categories),
                errors => Problem(errors));
        });
    }
}