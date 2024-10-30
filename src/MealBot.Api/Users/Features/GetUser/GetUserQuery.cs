namespace MealBot.Api.Users.Features.GetUser;

public sealed record GetUserQuery(Guid UserId) : IRequest<ErrorOr<User>> { }