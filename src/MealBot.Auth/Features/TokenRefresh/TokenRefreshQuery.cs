namespace MealBot.Auth.Features.TokenRefresh;

public sealed record TokenRefreshQuery(string OldAccessToken, string OldRefreshToken) : IRequest<ErrorOr<AccessToken>>;