namespace MealBot.Web.Features.User.Services;

internal interface IUserService
{
    Task<UserResponse?> GetUserAsync(CancellationToken cancellationToken);
}

internal sealed class UserService(
    IHttpClientFactory httpClientFactory,
    UserAuthenticationStateProvider authenticationStateProvider) : IUserService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly UserAuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

    public async Task<UserResponse?> GetUserAsync(CancellationToken cancellationToken)
    {
        if (!_authenticationStateProvider.CurrentUser.IsAuthenticated)
        {
            return null;
        }

        string userId = _authenticationStateProvider.CurrentUser.UserId!;

        using var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"users/{userId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            // FIXME: Do something when the request fails
            return null;
        }

        return await response.Content.ReadFromJsonAsync<UserResponse>(cancellationToken)
            ?? null;
    }
}