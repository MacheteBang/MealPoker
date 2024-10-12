
namespace MealBot.Web.Handlers;

public class TokenRefreshDelegatingHandler : PathFilterDelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (ShouldHandle(request))
        {
            // TODO: Implement token refresh logic
        }
        return base.SendAsync(request, cancellationToken);
    }
}