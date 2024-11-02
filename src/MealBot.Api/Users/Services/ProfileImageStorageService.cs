using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace MealBot.Api.Users.Services;

internal interface IProfileImageStorageService
{
    Task<ErrorOr<Uri>> SaveImageAsync(Guid userId, Stream imageStream);
    Task<ErrorOr<Uri>> SaveImageAsync(Guid userId, Uri sourceUri);
    [Obsolete]
    Task<ErrorOr<Stream>> GetImageAsync(Guid userId, int width);
    Task<ErrorOr<Success>> DeleteImageAsync(Guid userId);
}

internal sealed class LocalProfileImageStorageService(
    IHttpClientFactory httpClientFactory,
    IHttpContextAccessor httpContextAccessor) : IProfileImageStorageService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public Task<ErrorOr<Success>> DeleteImageAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Stream>> GetImageAsync(Guid userId, int width)
    {
        try
        {
            string filePath = GetFilePath(userId);
            using var image = Image.Load(filePath);
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            if (width > 0 && width < image.Width)
            {
                int height = (int)(originalHeight * ((float)width / originalWidth));
                var resizeOptions = new ResizeOptions
                {
                    Size = new Size(width, height),
                    Sampler = KnownResamplers.Lanczos3,
                    Mode = ResizeMode.Max
                };
                image.Mutate(x => x.Resize(resizeOptions));
            }

            var resizedStream = new MemoryStream();
            image.Save(resizedStream, new PngEncoder());
            resizedStream.Position = 0;

            return Task.FromResult<ErrorOr<Stream>>(resizedStream);
        }
        catch
        {
            return Task.FromResult<ErrorOr<Stream>>(Errors.GetProfileImageFailed());
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
                return Errors.SaveProfileImageFailed();
            }

            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

            return new Uri($"{baseUrl}{GlobalSettings.RoutePaths.Users}/{userId}/profile-image");
        }
        catch
        {
            return Errors.SaveProfileImageFailed();
        }
    }

    public async Task<ErrorOr<Uri>> SaveImageAsync(Guid userId, Uri sourceUri)
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage = await client.GetAsync(sourceUri);
        if (!responseMessage.IsSuccessStatusCode)
        {
            return Errors.SaveProfileImageFailed();
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