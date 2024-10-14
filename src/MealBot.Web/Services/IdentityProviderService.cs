using System.Net.Http.Json;
using System.Text.Json;

namespace MealBot.Web.Services;

public class IdentityProviderService(
    IHttpClientFactory httpClientFactory,
    NavigationManager navigationManager,
    IBrowserStorageService browserStorageService)
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly NavigationManager _navigationManager = navigationManager;
    private readonly IBrowserStorageService _browserStorageService = browserStorageService;

    public async Task AuthenticateWithGoogle()
    {
        var httpClient = _httpClientFactory.CreateClient();

        // Call our backend API to retrieve a URL for Google authentication.
        // Preserve the current URI in the state, so we can return the user back
        // to the same page after successful authentication
        string state = Uri.EscapeDataString(_navigationManager.Uri);
        string callBackUri = Uri.EscapeDataString($"{_navigationManager.BaseUri}google-callback");
        var httpResponseMessage = await httpClient.GetAsync($"auth/urls?provider=Google&state={state}&callbackUri={callBackUri}");

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            await _browserStorageService.RemoveAccessTokenAsync();

            // Navigate the user to the Google authentication page
            var authUrlResponse = await httpResponseMessage.Content.ReadFromJsonAsync<AuthUrlReponse>();
            if (authUrlResponse is null)
            {
                // TODO: Handle the error of not being able to deserialize the AuthUrlResponse
                return;
            }

            _navigationManager.NavigateTo(authUrlResponse.Url);
        }
    }
}
