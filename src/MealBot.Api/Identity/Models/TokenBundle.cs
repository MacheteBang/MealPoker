namespace MealBot.Api.Identity.Models;

internal sealed record TokenBundle(AccessToken AccessToken, RefreshToken RefreshToken);