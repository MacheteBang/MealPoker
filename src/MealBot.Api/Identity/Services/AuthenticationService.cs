namespace MealBot.Api.Identity.Services;

internal interface IAuthenticationService
{
    Task<ErrorOr<User>> AddOrGetUserAsync(ExternalIdentity externalIdentity);
}

internal sealed class AuthenticationService(
    IUserService _userRepository,
    IProfileImageStorageService profileImageStorageService) : IAuthenticationService
{
    private readonly IUserService userRepository = _userRepository;
    private readonly IProfileImageStorageService _profileImageStorageService = profileImageStorageService;

    public async Task<ErrorOr<User>> AddOrGetUserAsync(ExternalIdentity externalIdentity)
    {
        var userResult = await userRepository.GetByEmailAddressAsync(externalIdentity.EmailAddress);

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

            var addUserResult = await userRepository.AddAsync(newUser);

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

            var updateResult = await userRepository.UpdateAsync(foundUser);

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

            if (!saveResult.IsError)
            {
                user.ProfileImageUrl = saveResult.Value.ToString();
            }
        }
    }

    private static bool IsUserDifferent(User user, ExternalIdentity externalIdentity) =>
        user.AuthProvider != externalIdentity.AuthProvider ||
        user.ExternalId != externalIdentity.Id ||
        user.FirstName != externalIdentity.FirstName ||
        user.LastName != externalIdentity.LastName;
}