namespace MealBot.Auth.Services;

public interface IUserService
{
    Task<ErrorOr<User>> AddAsync(User user);
    Task<ErrorOr<User>> GetByEmailAddressAsync(string emailAddress);
    Task<ErrorOr<User>> UpdateAsync(User user);
    Task<ErrorOr<Success>> DeleteAsync(Guid userId);
}

public sealed class UserService : IUserService
{
    public Task<ErrorOr<User>> AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Success>> DeleteAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<User>> GetByEmailAddressAsync(string emailAddress)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<User>> UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }
}