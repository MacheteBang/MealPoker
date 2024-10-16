namespace MealBot.Api.Auth.Services;

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

            if (!string.IsNullOrEmpty(externalIdentity.ProfilePictureUri))
            {
                await SaveProfileImage(newUser.UserId, new Uri(externalIdentity.ProfilePictureUri));
            }

            var addUserResult = await userRepository.AddAsync(newUser);

            if (addUserResult.IsError)
            {
                return addUserResult;
            }

            return newUser;
        }

        if (!string.IsNullOrEmpty(externalIdentity.ProfilePictureUri))
        {
            await SaveProfileImage(userResult.Value.UserId, new Uri(externalIdentity.ProfilePictureUri));
        }

        if (IsUserDifferent(userResult.Value, externalIdentity))
        {
            var foundUser = userResult.Value;
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

        return userResult.Value;
    }

    private async Task SaveProfileImage(Guid userId, Uri profileImageUri)
    {
        var profileImageResult = await _profileImageStorageService.SaveImageAsync(
            userId,
            profileImageUri);

        if (!profileImageResult.IsError)
        {
            // TODO: Log this error when there is a problem saving the profile image.
        }
    }

    private static bool IsUserDifferent(User user, ExternalIdentity externalIdentity) =>
        user.AuthProvider != externalIdentity.AuthProvider ||
        user.ExternalId != externalIdentity.Id ||
        user.FirstName != externalIdentity.FirstName ||
        user.LastName != externalIdentity.LastName;
}