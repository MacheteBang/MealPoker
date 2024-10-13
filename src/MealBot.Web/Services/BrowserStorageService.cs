using Microsoft.JSInterop;

namespace MealBot.Web.Services;

public interface IBrowserStorageService
{
    Task SaveAccessTokenAsync(string accessToken);
    Task GetAccessTokenAsync();
    Task RemoveAccessTokenAsync();
}

public class BrowserStorageService(IJSRuntime jsRuntime) : IBrowserStorageService
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;

    private const string AccessTokenName = "MealBot.AccessToken";

    public async Task GetAccessTokenAsync()
        => await GetItemAsync(AccessTokenName);

    public Task RemoveAccessTokenAsync()
        => RemoveItemAsync(AccessTokenName);

    public Task SaveAccessTokenAsync(string accessToken)
        => SaveItemAsync(AccessTokenName, accessToken);


    private async Task SaveItemAsync(string key, string value)
        => await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);

    private async Task<string?> GetItemAsync(string key)
        => await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);

    private async Task RemoveItemAsync(string key)
        => await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
}