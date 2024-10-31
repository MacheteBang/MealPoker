namespace MealBot.Api.Families.Features.JoinFamily;

public record JoinFamilyCommand(Guid UserId, string Code) : IRequest<ErrorOr<Success>>;