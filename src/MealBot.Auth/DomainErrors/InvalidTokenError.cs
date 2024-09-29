namespace MealBot.Auth.DomainErrors;

public static partial class Errors
{
    public static Error InvalidToken() => Error.Validation(
        code: "Token.Invalid",
        description: $"The token provided was found to be invalid.");
}
