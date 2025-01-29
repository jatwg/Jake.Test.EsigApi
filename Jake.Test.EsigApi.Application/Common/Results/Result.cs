namespace Jake.Test.EsigApi.Application.Common.Results;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public bool IsNotFound { get; }

    private Result(bool isSuccess, T? value = default, string? error = null, bool isNotFound = false)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        IsNotFound = isNotFound;
    }

    public static Result<T> Success(T value) => new(true, value);
    public static Result<T> Failure(string error) => new(false, error: error);
    public static Result<T> NotFound(string? error = null) => new(false, error: error, isNotFound: true);
}

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public bool IsNotFound { get; }

    private Result(bool isSuccess, string? error = null, bool isNotFound = false)
    {
        IsSuccess = isSuccess;
        Error = error;
        IsNotFound = isNotFound;
    }

    public static Result Success() => new(true);
    public static Result Failure(string error) => new(false, error);
    public static Result NotFound(string? error = null) => new(false, error, true);
} 