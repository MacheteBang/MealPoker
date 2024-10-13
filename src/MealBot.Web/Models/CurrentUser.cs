namespace MealBot.Web.Models;

public class CurrentUser
{
    public string? UserId { get; set; }
    public bool IsAuthenticated { get; set; }
    public string? EmailAddress { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PictureUri { get; set; }
}