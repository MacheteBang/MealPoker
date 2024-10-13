var builder = WebAssemblyHostBuilder.CreateDefault(args);
{
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.AddScoped<IdentityProviderService>();
    builder.Services.AddScoped<IBrowserStorageService, BrowserStorageService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<UserAuthenticationStateProvider>();
    builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<UserAuthenticationStateProvider>());
    builder.Services.AddTransient<CookieDelegatingHandler>();
    ConfigureMealBotHttpClient(builder);
}

var app = builder.Build();
{

}

await app.RunAsync();


static void ConfigureMealBotHttpClient(WebAssemblyHostBuilder builder)
{
    builder.Services.AddTransient<TokenRefreshDelegatingHandler>();

    string baseApiUri = builder.Configuration.GetRequiredValue<string>("BaseApiUri");

    builder.Services.AddHttpClient("", (serviceProvider, httpClient) =>
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        httpClient.BaseAddress = new Uri(baseApiUri);
    })
    .AddHttpMessageHandler<CookieDelegatingHandler>()
    .AddHttpMessageHandler<TokenRefreshDelegatingHandler>();
}