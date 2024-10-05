namespace MealBot.Auth.DomainErrors;

public static partial class Errors
{
    public static Error ProviderNotSupported(string provider) => Error.Validation(
        code: "Auth.ProviderNotSupported",
        description: $"Given provider '{provider}' is not supported.");
}
