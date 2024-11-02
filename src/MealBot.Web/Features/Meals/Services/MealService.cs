using System.Text;

namespace MealBot.Web.Features.Meals.Services;

internal interface IMealService
{
    Task<ApiResult<List<MealResponse>>> GetMealsAsync(CancellationToken cancellationToken);
    Task<ApiResult<bool>> AddMealAsync(CreateMealRequest request, CancellationToken cancellationToken);
    Task<ApiResult<bool>> DeleteMealAsync(Guid id, CancellationToken cancellationToken);
    string? GetEmojiForCategory(string category);
    string? GetEmojiForCategory(MealPartCategory category);
}

internal sealed class MealService(IHttpClientFactory httpClientFactory) : IMealService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<ApiResult<List<MealResponse>>> GetMealsAsync(CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("meals", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return response.StatusCode.ToApiResultError();
        }

        var value = await response.Content.ReadFromJsonAsync<List<MealResponse>>(cancellationToken);
        if (value is null) return new ServerErrorApiResultError();

        return value;
    }

    public async Task<ApiResult<bool>> AddMealAsync(CreateMealRequest request, CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsync(
            "meals",
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"),
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return response.StatusCode.ToApiResultError();
        }

        return true;
    }


    public async Task<ApiResult<bool>> DeleteMealAsync(Guid id, CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();
        var response = await client.DeleteAsync(
            $"meals/{id}",
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return response.StatusCode.ToApiResultError();
        }

        return true;
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
    public string? GetEmojiForCategory(MealPartCategory category) => GetEmojiForCategory(category.ToString());
}