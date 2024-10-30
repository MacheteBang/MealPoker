namespace MealBot.Api.Families.Features.LeaveFamily;

public record LeaveFamilyCommand(Guid UserId, Guid FamilyId) : IRequest<ErrorOr<Success>>;