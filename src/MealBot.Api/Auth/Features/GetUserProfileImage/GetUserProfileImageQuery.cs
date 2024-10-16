namespace MealBot.Api.Auth.Features.GetUserProfileImage;

public record GetUserProfileImageQuery(Guid UserId) : IRequest<ErrorOr<Stream>>;