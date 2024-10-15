using Microsoft.EntityFrameworkCore;
using Errors = MealBot.Api.Auth.DomainErrors.Errors;

namespace MealBot.Api.Auth.Services;

internal interface IUserService
{
    Task<ErrorOr<User>> AddAsync(User user);
    Task<ErrorOr<User>> GetByEmailAddressAsync(string emailAddress);
    Task<ErrorOr<User>> UpdateAsync(User user);
    Task<ErrorOr<Success>> DeleteAsync(string emailAddress);
}

internal sealed class UserService(AuthDbContext dbContext) : IUserService
{
    private readonly AuthDbContext _dbContext = dbContext;

    public async Task<ErrorOr<User>> AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<ErrorOr<Success>> DeleteAsync(string emailAddress)
    {
        var userResult = await GetByEmailAddressAsync(emailAddress);
        if (userResult.IsError)
        {
            return userResult.Errors;
        }

        _dbContext.Users.Remove(userResult.Value);
        await _dbContext.SaveChangesAsync();

        return new Success();
    }

    public async Task<ErrorOr<User>> GetByEmailAddressAsync(string emailAddress)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.EmailAddress == emailAddress);

        return user != null
            ? user
            : Errors.UserNotFoundError();
    }

    public async Task<ErrorOr<User>> UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }
}