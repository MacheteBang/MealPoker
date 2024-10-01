namespace MealBot.Auth.Features.RedirectGoogle;

public record RedirectGoogleQuery(string State) : IRequest<ErrorOr<string>>;