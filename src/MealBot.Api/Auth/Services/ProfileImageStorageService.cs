using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

namespace MealBot.Api.Auth.Services;

internal interface IProfileImageStorageService
{
    Task<ErrorOr<Uri>> SaveImageAsync(Guid userId, Stream imageStream);
    Task<ErrorOr<Uri>> SaveImageAsync(Guid userId, Uri sourceUri);
    [Obsolete]
    Task<ErrorOr<Stream>> GetImageAsync(Guid userId);
    Task<ErrorOr<Success>> DeleteImageAsync(Guid userId);
}

internal sealed class LocalProfileImageStorageService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : IProfileImageStorageService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public Task<ErrorOr<Success>> DeleteImageAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Stream>> GetImageAsync(Guid userId)
    {
        try
        {
            string filePath = GetFilePath(userId);
            Stream imageStream = File.OpenRead(filePath);
            return Task.FromResult<ErrorOr<Stream>>(imageStream);
        }
        catch (Exception ex)
        {
            // TODO: Convert this to a standard Error
            return Task.FromResult<ErrorOr<Stream>>(Error.Failure("Auth.GetProfileImageFailed", ex.Message));
        }
    }

    public async Task<ErrorOr<Uri>> SaveImageAsync(Guid userId, Stream imageStream)
    {
        try
        {
            string filePath = GetFilePath(userId);

            // Save the stream to a file
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await imageStream.CopyToAsync(fileStream);
            }


            var request = _httpContextAccessor.HttpContext?.Request;
            if (request is null)
            {
                // TODO: Convert this to a standard Error
                return Error.Failure("Auth.SaveProfileImageFailed");
            }

            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

            return new Uri($"{baseUrl}{GlobalSettings.RoutePaths.Users}/{userId}/profile-image");
        }
        catch (Exception ex)
        {
            // TODO: Convert this to a standard Error
            return Error.Failure("Auth.SaveProfileImageFailed", ex.Message);
        }
    }

    public async Task<ErrorOr<Uri>> SaveImageAsync(Guid userId, Uri sourceUri)
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage = await client.GetAsync(sourceUri);
        if (!responseMessage.IsSuccessStatusCode)
        {
            // TODO: Convert this to a standard Error
            return Error.Failure("Auth.SaveProfileImageFailed", "Failed to download the image from the source URI.");
        }

        using (var imageStream = await responseMessage.Content.ReadAsStreamAsync())
        {
            return await SaveImageAsync(userId, imageStream);
        }
    }

    private static string GetFilePath(Guid userId)
    {
        // Define the path
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string directoryPath = Path.Combine(appDataPath, ".MealBot", "ProfileImages");
        string filePath = Path.Combine(directoryPath, $"{userId}.jpg");

        // Create directory if it doesn't exist
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        return filePath;
    }
}