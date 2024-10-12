using MealBot.Api.Common.Errors;

namespace MealBot.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSerilog();
        services.AddSingleton<ProblemDetailsFactory, MealBotProblemDetailsFactory>();
        return services;
    }

    public static WebApplication UsePresentation(this WebApplication app)
    {
        app.UseTestEndpoints();

        return app;
    }

    private static WebApplication UseTestEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapGet("/test/show-cookies", (HttpContext context) =>
            {
                return Results.Ok(context.Request.Cookies);
            });
        }

        return app;
    }
}