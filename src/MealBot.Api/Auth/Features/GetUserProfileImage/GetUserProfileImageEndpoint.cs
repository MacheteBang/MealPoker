using Microsoft.AspNetCore.Http.HttpResults;

namespace MealBot.Api.Auth.Features.GetUserProfileImage;

internal sealed class GetUserProfileImageEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{GlobalSettings.RoutePaths.Users}/{{userId:Guid}}/profile-image", async (string userId, ISender sender) =>
        {
            // TODO: Complete the implementation to return a file.
            throw new NotImplementedException();
        })
        .RequireAuthorization();
    }
}