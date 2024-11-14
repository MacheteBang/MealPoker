using System.Text;

namespace MealBot.Web.Features.Meals.Services;

internal interface IMealService
{
    Task<ApiResult<MealResponse>> GetMealAsync(Guid mealId, CancellationToken cancellationToken);
    Task<ApiResult<List<MealResponse>>> GetUserMealsAsync(CancellationToken cancellationToken);
    Task<ApiResult<List<MealResponse>>> GetFamilyMealsAsync(CancellationToken cancellationToken);
    Task<ApiResult<bool>> AddMealAsync(CreateMealRequest request, CancellationToken cancellationToken);
    Task<ApiResult<bool>> DeleteMealAsync(Guid id, CancellationToken cancellationToken);
    Task<ApiResult<bool>> RateMealAsync(Guid id, MealRating rating, CancellationToken cancellationToken);
    Task<ApiResult<bool>> UnrateMealAsync(Guid id, CancellationToken cancellationToken);
    string? GetEmojiForCategory(string category);
    string? GetEmojiForCategory(MealPartCategory category);
    string? GetEmojiForMealRating(string category);
    string? GetEmojiForMealRating(MealRating mealRating);
}

internal sealed class MealService(IHttpClientFactory httpClientFactory) : IMealService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<ApiResult<MealResponse>> GetMealAsync(Guid mealId, CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"meals/{mealId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return response.StatusCode.ToApiResultError();
        }

        var value = await response.Content.ReadFromJsonAsync<MealResponse>(cancellationToken);
        if (value is null) return new ServerErrorApiResultError();

        return value;
    }

    public async Task<ApiResult<List<MealResponse>>> GetUserMealsAsync(CancellationToken cancellationToken)
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

    public async Task<ApiResult<List<MealResponse>>> GetFamilyMealsAsync(CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("meals?family=true", cancellationToken);
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
    public async Task<ApiResult<bool>> RateMealAsync(Guid id, MealRating rating, CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsync(
            $"meals/{id}/ratings/{rating}",
            new StringContent(string.Empty),
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return response.StatusCode.ToApiResultError();
        }

        return true;
    }
    public async Task<ApiResult<bool>> UnrateMealAsync(Guid id, CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();
        var response = await client.DeleteAsync(
            $"meals/{id}/ratings",
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

    public string? GetEmojiForMealRating(string category)
    {
        Dictionary<string, string> dictionary = new()
        {
            { "Hate", "🤮" },
            { "Dislike", "😒" },
            { "Neutral", "😐" },
            { "Like", "😊" },
            { "Love", "😍" },
        };

        return dictionary.TryGetValue(category, out var emoji)
            ? emoji
            : null;
    }
    public string? GetEmojiForMealRating(MealRating mealRating) => GetEmojiForMealRating(mealRating.ToString());
}