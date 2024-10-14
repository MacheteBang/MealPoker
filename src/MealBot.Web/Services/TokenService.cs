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

        var tokenRefreshResponse = await client.PostAsJsonAsync("auth/tokens/refresh", oldToken, cancellationToken);
        if (tokenRefreshResponse.IsSuccessStatusCode)
        {
            var newAccessToken = await tokenRefreshResponse.Content.ReadAsStringAsync(cancellationToken);
            return newAccessToken;
        }
        else
        {
            return null;
        }
    }
}