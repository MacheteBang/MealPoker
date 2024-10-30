namespace MealBot.Api.Families.Features.GetFamily;

public record GetFamilyQuery(Guid UserId, Guid FamilyId) : IRequest<ErrorOr<Family>>;