namespace Habita.Domain.Common;

             
                                                                                                
              
public readonly struct Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    private Result(bool success, Error? error)
    {
        IsSuccess = success;
        Error = error;
    }

    public static Result Success() => new(true, null);

    public static Result Failure(Error error) => new(false, error);

    public Error GetError() => IsFailure ? Error! : throw new InvalidOperationException("No error on success.");

    public static Result<T> Success<T>(T value) => Result<T>.Success(value);

    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
}

             
                                                                        
                                                                                           
              
public readonly struct Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }
    public T? Value { get; }

    private Result(bool success, T? value, Error? error)
    {
        IsSuccess = success;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);

    public static Result<T> Failure(Error error) => new(false, default, error);

    public Error GetError() => IsFailure ? Error! : throw new InvalidOperationException("No error on success.");

    public T GetValue() => IsSuccess ? Value! : throw new InvalidOperationException("No value on failure.");

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess(Value!) : onFailure(Error!);
}
