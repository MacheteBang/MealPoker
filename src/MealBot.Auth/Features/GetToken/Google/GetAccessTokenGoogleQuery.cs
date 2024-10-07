namespace MealBot.Auth.Features.GetToken.Google;

public sealed record GetAccessTokenGoogleQuery(string AuthorizationCode, string CallBackUri) : IRequest<ErrorOr<AccessToken>>;