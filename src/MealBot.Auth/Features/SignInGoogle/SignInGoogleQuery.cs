namespace MealBot.Auth.Features.SignInGoogle;

public record SignInGoogleQuery(string AuthorizationToken) : IRequest<ErrorOr<AccessTokenSet>>;