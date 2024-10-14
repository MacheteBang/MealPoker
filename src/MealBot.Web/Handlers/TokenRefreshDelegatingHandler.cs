namespace MealBot.Web.Handlers;

public class TokenRefreshDelegatingHandler(
    IBrowserStorageService browserStorageService,
    ITokenService _tokenService) : PathFilterDelegatingHandler
{
    private readonly IBrowserStorageService _browserStorageService = browserStorageService;
    private readonly ITokenService _tokenService = _tokenService;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? accessTokenValue = await _browserStorageService.GetAccessTokenAsync();
        if (ShouldHandle(request) && !string.IsNullOrWhiteSpace(accessTokenValue))
        {
            JwtSecurityToken jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessTokenValue);

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                accessTokenValue = await _tokenService.RefreshTokenAsync(accessTokenValue, cancellationToken);
                if (!string.IsNullOrWhiteSpace(accessTokenValue))
                {
                    await _browserStorageService.SaveAccessTokenAsync(accessTokenValue);
                }
            }

            request.Headers.Authorization = new("Bearer", accessTokenValue);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}