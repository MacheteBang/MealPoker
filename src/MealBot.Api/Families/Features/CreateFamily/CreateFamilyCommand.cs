namespace MealBot.Api.Families.Features.CreateFamily;

public record CreateFamilyCommand(
    Guid UserId,
    string Name,
    string? Description
) : IRequest<ErrorOr<Family>>;