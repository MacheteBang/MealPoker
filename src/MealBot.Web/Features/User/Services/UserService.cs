namespace MealBot.Web.Features.User.Services;

internal interface IUserService
{
    Task<ApiResult<UserResponse>> GetUserAsync(CancellationToken cancellationToken);
}

internal sealed class UserService(
    IHttpClientFactory httpClientFactory,
    UserAuthenticationStateProvider authenticationStateProvider) : IUserService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly UserAuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

    public async Task<ApiResult<UserResponse>> GetUserAsync(CancellationToken cancellationToken)
    {
        if (!_authenticationStateProvider.CurrentUser.IsAuthenticated)
        {
            return new NotAuthenticatedApiResultError();
        }

        string userId = _authenticationStateProvider.CurrentUser.UserId!;

        using var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"users/{userId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return response.StatusCode.ToApiResultError();
        }

        var value = await response.Content.ReadFromJsonAsync<UserResponse>(cancellationToken);
        if (value is null) return new ServerErrorApiResultError();

        return value;
    }
}