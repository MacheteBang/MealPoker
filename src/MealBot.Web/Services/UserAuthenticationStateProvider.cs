using MealBot.Web.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MealBot.Web.Services;

public class UserAuthenticationStateProvider(
    IBrowserStorageService browserStorageService,
    ITokenService tokenService) : AuthenticationStateProvider
{
    private readonly IBrowserStorageService _browserStorageService = browserStorageService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ClaimsPrincipal _anonymousUser = new(new ClaimsIdentity());

    public CurrentUser CurrentUser { get; private set; } = new();

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }

    private async Task<ClaimsPrincipal> GetClaimsPrincipalAsync()
    {
        string tokenFromStorage = await _browserStorageService.GetAccessTokenAsync();
        if (string.IsNullOrWhiteSpace(tokenFromStorage))
        {
            CurrentUser = new();
            return _anonymousUser;
        }

        var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenFromStorage);
        if (accessToken.ValidTo < DateTime.UtcNow)
        {
            var refreshAccessToken = await _tokenService.RefreshTokenAsync(tokenFromStorage, CancellationToken.None);
            if (string.IsNullOrWhiteSpace(refreshAccessToken))
            {
                CurrentUser = new();
                return _anonymousUser;
            }

            await _browserStorageService.SaveAccessTokenAsync(refreshAccessToken);
        }

        ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity(accessToken.Claims, "MealBot"));
        UpdateCurrentUser(claimsPrincipal);
        return claimsPrincipal;
    }

    public async Task NotifyUserLoggedin()
    {
        var principal = await GetClaimsPrincipalAsync();
        var authState = Task.FromResult(new AuthenticationState(principal));
        NotifyAuthenticationStateChanged(authState);
    }

    public async Task NotifyUserLoggedOut()
    {
        await _browserStorageService.RemoveAccessTokenAsync();
        var authState = Task.FromResult(new AuthenticationState(_anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    private void UpdateCurrentUser(ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity?.IsAuthenticated == true)
        {
            CurrentUser = new()
            {
                UserId = claimsPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                IsAuthenticated = true,
                EmailAddress = claimsPrincipal.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
                FirstName = claimsPrincipal.FindFirst(c => c.Type == ClaimTypes.GivenName)?.Value,
                LastName = claimsPrincipal.FindFirst(c => c.Type == ClaimTypes.Surname)?.Value,
                PictureUri = claimsPrincipal.FindFirst(c => c.Type == "profilepicture")?.Value
            };
        }
        else
        {
            CurrentUser = new();
        }
    }
}