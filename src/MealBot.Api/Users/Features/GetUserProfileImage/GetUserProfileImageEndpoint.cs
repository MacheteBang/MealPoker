namespace MealBot.Api.Users.Features.GetUserProfileImage;

internal sealed class GetUserProfileImageEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{GlobalSettings.RoutePaths.Users}/{{userId:Guid}}{GlobalSettings.RoutePaths.ProfileImages}", async (
            Guid userId,
            ISender sender,
            [FromQuery] ushort width = 32) =>
        {
            var query = new GetUserProfileImageQuery(userId, width);
            var result = await sender.Send(query);

            return result.Match(
                stream => Results.File(stream, "image/png"),
                errors => Problem(errors));
        });
    }
}