using System.Net;

namespace MealBot.Web.Common;

public readonly struct ApiResult<TValue>
{
    public bool IsProcessing { get; }
    public bool IsError { get; }
    public bool HasValue => !IsProcessing && !IsError;
    public TValue Value => _value ?? default!;
    public ApiResultError Error => _error ?? default!;

    private readonly TValue? _value;
    private readonly ApiResultError? _error;

    public ApiResult()
    {
        IsProcessing = true;
    }

    private ApiResult(TValue value)
    {
        IsError = false;
        _value = value;
        _error = default;
    }

    private ApiResult(ApiResultError error)
    {
        IsError = true;
        _error = error;
        _value = default;
    }

    public static implicit operator ApiResult<TValue>(TValue value) => new(value);
    public static implicit operator ApiResult<TValue>(ApiResultError error) => new(error);
}

public abstract class ApiResultError { }
public class NotFoundApiResultError : ApiResultError { }
public class NotAuthenticatedApiResultError : ApiResultError { }
public class ServerErrorApiResultError : ApiResultError { }
public class BadRequestApiResultError : ApiResultError { }

public static class HttpStatusCodeExtensions
{
    public static ApiResultError ToApiResultError(this HttpStatusCode statusCode) => statusCode switch
    {
        >= HttpStatusCode.InternalServerError => new ServerErrorApiResultError(),
        HttpStatusCode.Unauthorized => new NotAuthenticatedApiResultError(),
        HttpStatusCode.NotFound => new NotFoundApiResultError(),
        _ => new BadRequestApiResultError()
    };
}
