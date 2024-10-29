namespace MealBot.Api.Identity.Features.GetAuthUrl;

public sealed class GetAuthUrlQuery : IRequest<ErrorOr<string>>
{
    public AuthProvider Provider { get; set; }
    public required string State { get; set; }
    public required string CallbackUri { get; set; }
}