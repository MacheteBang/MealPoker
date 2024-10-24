namespace MealBot.Web.Features.Meals.Services;

internal interface IMealService
{
    Task<List<MealResponse>> GetMealsAsync(CancellationToken cancellationToken);
    string? GetEmojiForCategory(string category);
}

internal sealed class MealService(IHttpClientFactory httpClientFactory) : IMealService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<List<MealResponse>> GetMealsAsync(CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("meals", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return [];
        }

        return await response.Content.ReadFromJsonAsync<List<MealResponse>>(cancellationToken)
            ?? [];
    }

    public string? GetEmojiForCategory(string category)
    {
        Dictionary<string, string> dictionary = new()
        {
            { "MainCourse", "🍲" },
            { "Vegetable", "🥦" },
            { "SideDish", "🍚" },
            { "Dessert", "🍰" },
            { "Drink", "🥤" },
            { "Other", "🍽️" }
        };

        return dictionary.TryGetValue(category, out var emoji)
            ? emoji
            : null;
    }

}