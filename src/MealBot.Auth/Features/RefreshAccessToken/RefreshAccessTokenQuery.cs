namespace MealBot.Auth.Features.RefreshAccessToken;

public record RefreshAccessTokenQuery(string AccessToken, string RefreshToken) : IRequest<ErrorOr<AccessToken>>;