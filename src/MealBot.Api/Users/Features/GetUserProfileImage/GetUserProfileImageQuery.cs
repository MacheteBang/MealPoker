namespace MealBot.Api.Users.Features.GetUserProfileImage;

public record GetUserProfileImageQuery(Guid UserId, ushort Width) : IRequest<ErrorOr<Stream>>;