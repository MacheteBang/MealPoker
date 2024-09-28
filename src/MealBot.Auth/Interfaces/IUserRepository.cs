namespace MealBot.Auth.Interfaces;

public interface IUserRepository
{
    Task<ErrorOr<User>> AddUserAsync(User user);
    Task<ErrorOr<User>> GetUserByIdAsync(Guid userId);
    Task<ErrorOr<User>> GetUserByExternalIdAsync(string externalId);
    Task<ErrorOr<User>> GetUserByEmailAddressAsync(string emailAddress);
    Task<ErrorOr<User>> UpdateUser(User user);
    Task<ErrorOr<Success>> DeleteUser(Guid userId);
}