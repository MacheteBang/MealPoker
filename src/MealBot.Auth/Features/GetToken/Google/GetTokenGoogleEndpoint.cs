namespace MealBot.Auth.Features.GetToken.Google;

internal sealed class GetTokenEndpoint : AuthEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Globals.GetTokenGoogleRoute, async (
            string authorizationCode,
            string callBackUri,
            ISender sender) =>
        {
            if (string.IsNullOrWhiteSpace(authorizationCode))
            {
                return Results.Unauthorized();
            }

            var query = new GetTokenGoogleQuery(authorizationCode, callBackUri);
            var result = await sender.Send(query);

            // TODO: Append the refresh token to the Cookies

            TokenResponse tokenResponse = new(result.Value.AccessToken);

            return result.Match(
                token => Results.Ok(tokenResponse),
                _ => Results.Unauthorized());
        });
    }
}