namespace MealBot.Api.Auth.Features.GetUserProfileImage;

public record GetUserProfileImageQuery(string UserId) : IRequest<ErrorOr<Stream>>;