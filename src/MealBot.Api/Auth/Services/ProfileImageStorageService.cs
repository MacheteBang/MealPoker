using System.Security.Cryptography;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

namespace MealBot.Api.Auth.Services;

internal interface IProfileImageStorageService
{
    Task<ErrorOr<Success>> SaveImageAsync(string userId, Stream imageStream);
    Task<ErrorOr<Success>> SaveImageAsync(string userId, Uri sourceUri);
    Task<ErrorOr<Stream>> GetImageAsync(string userId);
    Task<ErrorOr<Success>> DeleteImageAsync(string userId);
}

internal sealed class LocalProfileImageStorageService(IHttpClientFactory httpClientFactory) : IProfileImageStorageService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public Task<ErrorOr<Success>> DeleteImageAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Stream>> GetImageAsync(string userId)
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

    public async Task<ErrorOr<Success>> SaveImageAsync(string userId, Stream imageStream)
    {
        try
        {
            string filePath = GetFilePath(userId);

            // Save the stream to a file
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await imageStream.CopyToAsync(fileStream);
            }

            return new Success();
        }
        catch (Exception ex)
        {
            // TODO: Convert this to a standard Error
            return Error.Failure("Auth.SaveProfileImageFailed", ex.Message);
        }
    }

    public async Task<ErrorOr<Success>> SaveImageAsync(string userId, Uri sourceUri)
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

    private static string HashUserId(string userId)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(userId));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
    private static string GetFilePath(string userId)
    {
        // Hash the userId
        string hashedUserId = HashUserId(userId);

        // Define the path
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string directoryPath = Path.Combine(appDataPath, "MealBot", "ProfileImages");
        string filePath = Path.Combine(directoryPath, $"{hashedUserId}.jpg");

        // Create directory if it doesn't exist
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        return filePath;
    }
}