using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MealBot.Common;

public abstract class EndpointBase : IEndpoint
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

    private IResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Results.Problem(statusCode: statusCode, title: error.Description);
    }

    private IResult ValidationProblem(List<Error> errors)
    {
        var validationErrorDictionary = errors.ToDictionary(e => e.Code, e => new[] { e.Description });

        return Results.ValidationProblem(validationErrorDictionary);
    }
}