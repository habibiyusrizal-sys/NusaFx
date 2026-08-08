using System;

namespace NusaFx.Application.Common;

public class Result<T>
{
    public T Data { get; private set; }
    public bool IsSuccess { get; private set; }
    public string ErrorMessage { get; private set; }

    private Result(T data, bool isSuccess, string errorMessage)
    {
        Data = data;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T data)
    {
        return new Result<T>(data, true, null);
    }

    public static Result<T> Failure(string errorMessage)
    {
        return new Result<T>(default(T), false, errorMessage);
    }
}
