namespace MealBot.Meals.Features.GetMealPartCategories;

public sealed class GetMealPartCategoriesEndpoint : MealsEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Globals.BaseMealPartCategoryRoute, async (ISender sender) =>
        {
            var result = await sender.Send(new GetMealPartCategoriesQuery());

            return result.Match(
                categories => Results.Ok(categories),
                errors => Problem(errors));
        });
    }
}