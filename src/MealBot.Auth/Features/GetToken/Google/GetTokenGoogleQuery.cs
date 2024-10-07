namespace MealBot.Auth.Features.GetToken.Google;

public sealed record GetTokenGoogleQuery(string AuthorizationCode, string CallBackUri) : IRequest<ErrorOr<Token>>;