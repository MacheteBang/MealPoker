namespace MealBot.Auth.Models;

public class User
{
    public required string EmailAddress { get; set; }
    public AuthProvider AuthProvider { get; set; }
    public required string ExternalId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PictureUri { get; set; }
}