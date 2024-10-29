namespace MealBot.Api.Identity.Features.GetUser;

public sealed record GetUserQuery(Guid UserId) : IRequest<ErrorOr<User>> { }