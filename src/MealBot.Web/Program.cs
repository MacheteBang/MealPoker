var builder = WebAssemblyHostBuilder.CreateDefault(args);
{
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.AddScoped<IdentityProviderService>();
    builder.Services.AddHttpClient("", (serviceProvider, httpClient) =>
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        httpClient.BaseAddress = new Uri(configuration["BaseApiUri"]!);
    })
    .AddHttpMessageHandler<CookieDelegatingHandler>()
    .AddHttpMessageHandler<TokenRefreshDelegatingHandler>();
}

var app = builder.Build();
{

}

await app.RunAsync();
