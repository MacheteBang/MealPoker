using Errors = MealBot.Api.Auth.DomainErrors.Errors;

namespace MealBot.Api.Auth.Features.GetAuthUrl;

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
                CallbackUri = callbackUri
            };

            var result = await sender.Send(query);

            return result.Match(
                url => Results.Ok(new AuthUrlReponse(provider.ToString(), url)),
                error => Problem(error));
        });
    }
}