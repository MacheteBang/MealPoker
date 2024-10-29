namespace MealBot.Api.Identity.Features.GetUserProfileImage;

public record GetUserProfileImageQuery(Guid UserId) : IRequest<ErrorOr<Stream>>;