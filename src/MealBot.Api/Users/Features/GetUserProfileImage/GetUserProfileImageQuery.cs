namespace MealBot.Api.Users.Features.GetUserProfileImage;

public record GetUserProfileImageQuery(Guid UserId) : IRequest<ErrorOr<Stream>>;