namespace MealBot.Web.Handlers;

public abstract class PathFilterDelegatingHandler : DelegatingHandler
{
    /// <summary>
    /// Collection of API paths that should not be intercepted by the handler.
    /// </summary>
    private static readonly List<string> IgnoredPaths =
    [
        "auth/urls",
        "auth/tokens/google",
        "auth/tokens/refresh"
    ];

    /// <summary>
    /// Checks if the outgoing request should be intercepted.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    protected static bool ShouldHandle(HttpRequestMessage request) =>
        IgnoredPaths.All(path => !request.RequestUri!.AbsolutePath.Contains(path));
}