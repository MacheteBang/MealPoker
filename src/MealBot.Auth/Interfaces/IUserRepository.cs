namespace MealBot.Auth.Interfaces;

public interface IUserRepository
{
    Task<ErrorOr<User>> AddAsync(User user);
    Task<ErrorOr<User>> GetByEmailAddressAsync(string emailAddress);
    Task<ErrorOr<User>> UpdateAsync(User user);
    Task<ErrorOr<Success>> DeleteAsync(Guid userId);
}