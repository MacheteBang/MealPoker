//TODO: Use the actual contracts from the MealBot.Auth project
namespace MealBot.Web.Contracts;

public sealed record TokenResponse(string AccessToken, RefreshTokenResponse RefreshToken);