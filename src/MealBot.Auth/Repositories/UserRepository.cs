namespace MealBot.Auth.Repositories;

public sealed class UserRepository : IUserRepository
{
    public Task<ErrorOr<User>> AddUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Success>> DeleteUser(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<User>> GetUserByEmailAddressAsync(string emailAddress)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<User>> GetUserByExternalIdAsync(string externalId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<User>> GetUserByIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<User>> UpdateUser(User user)
    {
        throw new NotImplementedException();
    }
}