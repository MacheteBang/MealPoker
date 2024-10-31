namespace MealBot.Api.Families.Features.JoinFamily;

public record JoinFamilyCommand(Guid UserId, Guid FamilyId, string Code) : IRequest<ErrorOr<Success>>;