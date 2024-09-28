namespace MealBot.Auth.DomainErrors;

public static partial class Errors
{
    public static Error InvalidTokenIssuerError(string issuer) => Error.Validation(
        code: "Token.InvalidIssuer",
        description: $"While validating the access token, the issuer {issuer} was found to be invalid.");
}
