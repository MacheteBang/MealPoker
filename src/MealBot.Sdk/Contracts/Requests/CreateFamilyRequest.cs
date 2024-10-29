namespace MealBot.Sdk.Contracts.Requests;

public record CreateFamilyRequest(
    string Name,
    string? Description
);