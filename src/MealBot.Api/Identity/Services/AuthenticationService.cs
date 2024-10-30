namespace MealBot.Api.Identity.Services;

internal interface IAuthenticationService
{
    Task<ErrorOr<User>> AddOrGetUserAsync(ExternalIdentity externalIdentity);
}

internal sealed class AuthenticationService(
    IUserService _userService,
    IProfileImageStorageService profileImageStorageService) : IAuthenticationService
{
    private readonly IUserService _userService = _userService;
    private readonly IProfileImageStorageService _profileImageStorageService = profileImageStorageService;

    public async Task<ErrorOr<User>> AddOrGetUserAsync(ExternalIdentity externalIdentity)
    {
        var userResult = await _userService.GetByEmailAddressAsync(externalIdentity.EmailAddress);

        if (userResult.IsError && userResult.FirstError.Type != ErrorType.NotFound)
        {
            return userResult;
        }

        if (userResult.IsError && userResult.FirstError.Type == ErrorType.NotFound)
        {
            var newUser = User.Create(
                externalIdentity.EmailAddress,
                externalIdentity.AuthProvider,
                externalIdentity.Id,
                externalIdentity.FirstName,
                externalIdentity.LastName);

            await SaveProfileImageAsync(externalIdentity, newUser);

            var addUserResult = await _userService.AddAsync(newUser);

            if (addUserResult.IsError)
            {
                return addUserResult;
            }

            return newUser;
        }


        var foundUser = userResult.Value;

        await SaveProfileImageAsync(externalIdentity, foundUser);

        if (IsUserDifferent(foundUser, externalIdentity))
        {
            foundUser.AuthProvider = externalIdentity.AuthProvider;
            foundUser.ExternalId = externalIdentity.Id;
            foundUser.FirstName = externalIdentity.FirstName;
            foundUser.LastName = externalIdentity.LastName;

            var updateResult = await _userService.UpdateAsync(foundUser);

            return updateResult.Match<ErrorOr<User>>(
                user => user,
                errors => errors
            );
        }

        return foundUser;
    }

    private async Task SaveProfileImageAsync(ExternalIdentity externalIdentity, User user)
    {
        if (Uri.TryCreate(externalIdentity.ProfilePictureUri, UriKind.Absolute, out var profileImageUri))
        {
            var saveResult = await _profileImageStorageService.SaveImageAsync(
                user.UserId,
                profileImageUri);

            if (saveResult.IsError)
            {
                throw new Exception("Failed to save profile image");
            }
        }
    }

    private static bool IsUserDifferent(User user, ExternalIdentity externalIdentity) =>
        user.AuthProvider != externalIdentity.AuthProvider ||
        user.ExternalId != externalIdentity.Id ||
        user.FirstName != externalIdentity.FirstName ||
        user.LastName != externalIdentity.LastName;
}