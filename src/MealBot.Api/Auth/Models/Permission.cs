namespace MealBot.Api.Auth.Models;

internal sealed class Permission
{
    public required Guid UserId { get; init; }
    public required Guid Subject { get; init; }
    public required string SubjectType { get; init; }
}