namespace MealBot.Api.Auth.Features.GetUser;

public sealed record GetUserQuery(Guid UserId) : IRequest<ErrorOr<User>> { }