namespace MealBot.Auth.Features.GetAuthUrl;

public sealed class GetAuthUrlEndpoint : AuthEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Globals.AuthUrlRoute, async (
            AuthProvider provider,
            string state,
            string callbackUrl,
            ISender sender) =>
        {
            // TODO: Implement global error handling on the API for QSPs that are missing / empty
            // TODO: Implement global error handling on the API for invalid enum values

            if (provider == AuthProvider.Unknown)
            {
                return Problem(Errors.ProviderNotSupported());
            }

            var query = new GetAuthUrlQuery
            {
                Provider = provider,
                State = state,
                CallbackUrl = callbackUrl
            };

            var result = await sender.Send(query);

            return result.Match(
                url => Results.Ok(new AuthUrlReponse(provider, url)),
                error => Problem(error));
        });
    }
}