namespace MealBot.Auth.Features.GetSignInLocation;

public sealed class GetSignInLocationQuery : IRequest<ErrorOr<string>>
{
    public Provider Provider { get; set; }
    public required string State { get; set; }
    public required string ReturnUrl { get; set; }
}