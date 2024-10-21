namespace MealBot.Api.Common;

public interface IMealBotEndpoint
{
    void AddRoutes(IEndpointRouteBuilder app);
}

public abstract class MealBotEndpoint : IMealBotEndpoint
{
    public abstract void AddRoutes(IEndpointRouteBuilder app);

    protected IResult Problem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Results.Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors.First());
    }

    protected static IResult Problem(Error error)
    {
        if (error.Type == ErrorType.Validation)
        {
            return ValidationProblem([error]);
        }

        var statusCode = error.Type switch
        {
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Results.Problem(
            type: error.Code,
            title: error.Description,
            detail: error.Description,
            statusCode: statusCode);
    }

    private static IResult ValidationProblem(List<Error> errors)
    {
        var validationErrorDictionary = errors
            .GroupBy(e => e.Code)
            .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());

        return Results.ValidationProblem(validationErrorDictionary);
    }
}