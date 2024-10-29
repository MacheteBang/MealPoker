namespace MealBot.Api.Users.Services;

internal interface IUserService
{
    Task<ErrorOr<User>> AddAsync(User user);
    Task<ErrorOr<User>> GetByUserIdAsync(Guid userId);
    Task<ErrorOr<User>> GetByEmailAddressAsync(string emailAddress);
    Task<ErrorOr<User>> UpdateAsync(User user);
    Task<ErrorOr<Success>> DeleteAsync(Guid userId);
}

internal sealed class UserService(MealBotDbContext dbContext) : IUserService
{
    private readonly MealBotDbContext _dbContext = dbContext;

    public async Task<ErrorOr<User>> AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<ErrorOr<Success>> DeleteAsync(Guid userId)
    {
        var userResult = await GetByUserIdAsync(userId);
        if (userResult.IsError)
        {
            return userResult.Errors;
        }

        _dbContext.Users.Remove(userResult.Value);
        await _dbContext.SaveChangesAsync();

        return new Success();
    }

    public async Task<ErrorOr<User>> GetByUserIdAsync(Guid userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserId == userId);

        return user != null
            ? user
            : Errors.UserNotFoundError();
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