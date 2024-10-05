namespace MealBot.Auth.Features.GetSignInLocation;

public sealed class GetSignInLocationEndpoint : AuthEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Globals.SignInLocationRoute, async (
            Provider provider,
            string state,
            string returnUrl,
            ISender sender) =>
        {
            // TODO: Implement global error handling on the API for QSPs that are missing / empty
            // TODO: Implement global error handling on the API for invalid enum values

            if (provider == Provider.Unknown)
            {
                return Problem(Errors.ProviderNotSupported());
            }

            var query = new GetSignInLocationQuery
            {
                Provider = provider,
                State = state,
                ReturnUrl = returnUrl
            };

            var result = await sender.Send(query);

            return result.Match(
                url => Results.Ok(new SignInLocationResponse(provider, url)),
                error => Problem(error));
        });
    }
}