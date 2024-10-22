namespace MealBot.Web.Services;

internal interface IMealService
{
    Task<List<MealResponse>> GetMealsAsync(CancellationToken cancellationToken);
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
}