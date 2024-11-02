namespace MealBot.Web.Services;

internal interface IFamilyService
{
    Task<ApiResult<FamilyResponse>> GetFamilyAsync(Guid familyId, CancellationToken cancellationToken);
    Task<ApiResult<FamilyResponse>> CreateFamilyAsync(string name, string? description, CancellationToken cancellationToken);
    Task<ApiResult<bool>> LeaveFamilyAsync(Guid familyId, CancellationToken cancellationToken);
    Task<ApiResult<bool>> JoinFamilyAsync(string familyCode, CancellationToken cancellationToken);
}

internal sealed class FamilyService(
    IHttpClientFactory httpClientFactory,
    UserAuthenticationStateProvider authenticationStateProvider) : IFamilyService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly UserAuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

    public async Task<ApiResult<FamilyResponse>> CreateFamilyAsync(string name, string? description, CancellationToken cancellationToken)
    {
        string userId = _authenticationStateProvider.CurrentUser.UserId!;

        using var client = _httpClientFactory.CreateClient();
        var request = new CreateFamilyRequest(name, description);
        var httpResponse = await client.PostAsJsonAsync($"users/{userId}/families", request, cancellationToken);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return httpResponse.StatusCode.ToApiResultError();
        }

        var value = await httpResponse.Content.ReadFromJsonAsync<FamilyResponse>(cancellationToken);
        if (value is null) return new ServerErrorApiResultError();

        return value;
    }

    public async Task<ApiResult<FamilyResponse>> GetFamilyAsync(Guid familyId, CancellationToken cancellationToken)
    {
        string userId = _authenticationStateProvider.CurrentUser.UserId!;

        using var client = _httpClientFactory.CreateClient();
        var httpResponse = await client.GetAsync($"users/{userId}/families/{familyId}", cancellationToken);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return httpResponse.StatusCode.ToApiResultError();
        }

        var value = await httpResponse.Content.ReadFromJsonAsync<FamilyResponse>(cancellationToken);
        if (value is null) return new ServerErrorApiResultError();

        return value;
    }

    public async Task<ApiResult<bool>> JoinFamilyAsync(string familyCode, CancellationToken cancellationToken)
    {
        string userId = _authenticationStateProvider.CurrentUser.UserId!;

        using var client = _httpClientFactory.CreateClient();
        var httpResponse = await client.PatchAsync(
            $"users/{userId}/families/join/{familyCode}",
            new StringContent(""),
            cancellationToken);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return httpResponse.StatusCode.ToApiResultError();
        }

        return true;
    }

    public async Task<ApiResult<bool>> LeaveFamilyAsync(Guid familyId, CancellationToken cancellationToken)
    {
        string userId = _authenticationStateProvider.CurrentUser.UserId!;

        using var client = _httpClientFactory.CreateClient();
        var httpResponse = await client.DeleteAsync($"users/{userId}/families/{familyId}", cancellationToken);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return httpResponse.StatusCode.ToApiResultError();
        }

        return true;
    }
}