namespace MealBot.Auth.Features.GetUser;

public sealed record GetUserQuery(string EmailAddress) : IRequest<ErrorOr<User>> { }