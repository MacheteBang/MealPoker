namespace MealBot.Web.Services;

internal interface IFamilyService
{
    Task<FamilyResponse?> GetFamilyAsync(Guid familyId, CancellationToken cancellationToken);
    Task<FamilyResponse?> CreateFamilyAsync(string name, string? description, CancellationToken cancellationToken);
}

internal sealed class FamilyService(
    IHttpClientFactory httpClientFactory,
    UserAuthenticationStateProvider authenticationStateProvider) : IFamilyService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly UserAuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

    public async Task<FamilyResponse?> CreateFamilyAsync(string name, string? description, CancellationToken cancellationToken)
    {
        if (!_authenticationStateProvider.CurrentUser.IsAuthenticated)
        {
            return null;
        }

        string userId = _authenticationStateProvider.CurrentUser.UserId!;

        using var client = _httpClientFactory.CreateClient();
        var request = new CreateFamilyRequest(name, description);
        var httpResponse = await client.PostAsJsonAsync($"users/{userId}/families", request, cancellationToken);
        if (!httpResponse.IsSuccessStatusCode)
        {
            // FIXME: Do something when the request fails
            return null;
        }

        return await httpResponse.Content.ReadFromJsonAsync<FamilyResponse>(cancellationToken)
            ?? null;
    }

    public async Task<FamilyResponse?> GetFamilyAsync(Guid familyId, CancellationToken cancellationToken)
    {
        if (!_authenticationStateProvider.CurrentUser.IsAuthenticated)
        {
            return null;
        }

        string userId = _authenticationStateProvider.CurrentUser.UserId!;

        using var client = _httpClientFactory.CreateClient();
        var httpResponse = await client.GetAsync($"users/{userId}/families/{familyId}", cancellationToken);
        if (!httpResponse.IsSuccessStatusCode)
        {
            // FIXME: Do something when the request fails
            return null;
        }

        return await httpResponse.Content.ReadFromJsonAsync<FamilyResponse>(cancellationToken)
            ?? null;
    }
}