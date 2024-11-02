namespace MealBot.Api.Identity.Features.GetAuthUrl;

internal sealed class GetAuthUrlEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(GlobalSettings.RoutePaths.AuthUrls, async (
            AuthProvider provider,
            string state,
            string callbackUri,
            ISender sender) =>
        {
            if (provider == AuthProvider.Unknown)
            {
                return Problem(Errors.ProviderNotSupported());
            }

            var query = new GetAuthUrlQuery
            {
                Provider = provider,
                State = state,
                CallbackUri = callbackUri
            };

            var result = await sender.Send(query);

            return result.Match(
                url => Results.Ok(new AuthUrlResponse(provider.ToString(), url)),
                error => Problem(error));
        });
    }
}