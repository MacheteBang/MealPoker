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
            // TODO: Handle API Errors more gracefully
            throw new Exception($"`GET meals` failed with status code {response.StatusCode}");
        }

        return await response.Content.ReadFromJsonAsync<List<MealResponse>>(cancellationToken)
            ?? throw new NullReferenceException($"`GET meals` returned null");
    }
}