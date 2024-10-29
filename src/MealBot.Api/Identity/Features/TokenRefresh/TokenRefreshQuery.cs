namespace MealBot.Api.Identity.Features.TokenRefresh;

public sealed record TokenRefreshQuery(string OldAccessToken, string OldRefreshToken) : IRequest<ErrorOr<TokenBundle>>;