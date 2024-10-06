namespace MealBot.Auth.Repositories;

public sealed class UserRepository : IUserRepository
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