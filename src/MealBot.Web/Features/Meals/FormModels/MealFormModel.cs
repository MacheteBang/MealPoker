using MealBot.Sdk.ValueObjects;

namespace MealBot.Web.Features.Meals.FormModels;

public class MealFormModel
{
    public required string Name { get; set; }
    public string Description { get; set; } = "";
    public List<MealPartFormModel> Parts { get; set; } = [];

    public class MealPartFormModel
    {
        public MealPartCategory Category { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
    }
}