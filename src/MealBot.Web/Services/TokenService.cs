using System.Net.Http.Json;

namespace MealBot.Web.Services;

public interface ITokenService
{
    Task<string?> RefreshTokenAsync(string oldToken, CancellationToken cancellationToken);
}

public class TokenService(IHttpClientFactory httpClientFactory) : ITokenService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<string?> RefreshTokenAsync(string oldToken, CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();

        var tokenRefreshResponse = await client.PostAsJsonAsync("identity/tokens/refresh", new TokenRefreshRequest(oldToken), cancellationToken);
        if (!tokenRefreshResponse.IsSuccessStatusCode)
        {
            return null;
        }

        var newAccessToken = await tokenRefreshResponse.Content.ReadFromJsonAsync<AccessTokenResponse>(cancellationToken);
        if (newAccessToken == null)
        {
            return null;
        }

        return newAccessToken.Value;
    }
}