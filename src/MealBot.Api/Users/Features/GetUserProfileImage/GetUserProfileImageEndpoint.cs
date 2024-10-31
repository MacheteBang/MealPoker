namespace MealBot.Api.Users.Features.GetUserProfileImage;

internal sealed class GetUserProfileImageEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // TODO: Allow resizing of profile image at server to reduce bandwidth to the client
        app.MapGet($"{GlobalSettings.RoutePaths.Users}/{{userId:Guid}}{GlobalSettings.RoutePaths.ProfileImages}", async (Guid userId, ISender sender) =>
        {
            var query = new GetUserProfileImageQuery(userId);
            var result = await sender.Send(query);

            return result.Match(
                stream => Results.File(stream, "image/jpeg"),
                errors => Problem(errors));
        });
    }
}