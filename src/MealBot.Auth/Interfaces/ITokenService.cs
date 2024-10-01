namespace MealBot.Auth.Interfaces;

public interface ITokenService
{
    ErrorOr<string> GenerateAccessToken(User user);
    ErrorOr<RefreshToken> GenerateRefreshToken(User user);
    ErrorOr<Guid> GetUserId(string accessToken);
}