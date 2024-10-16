
namespace MealBot.Api.Auth.Features.GetUserProfileImage;

internal class GetUserProfileImageHandler(IProfileImageStorageService _profileImageStorageService) : IRequestHandler<GetUserProfileImageQuery, ErrorOr<Stream>>
{
    private readonly IProfileImageStorageService _profileImageStorageService = _profileImageStorageService;

    public async Task<ErrorOr<Stream>> Handle(GetUserProfileImageQuery query, CancellationToken cancellationToken)
    {
        var result = await _profileImageStorageService.GetImageAsync(query.UserId);

        return result.Match<ErrorOr<Stream>>(
            success => success,
            errors => errors
        );
    }
}