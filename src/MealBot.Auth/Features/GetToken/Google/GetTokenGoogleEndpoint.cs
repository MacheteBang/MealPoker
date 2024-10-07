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

            return result.Match(
                token => Results.Ok(token),
                _ => Results.Unauthorized());
        });
    }
}