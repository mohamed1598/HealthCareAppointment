using Shared.Primitives;

namespace Shared.Result;

public class Result<TValue> : Result
{
    protected Result() { }
    private readonly TValue? _value;
    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    => _value = value;

    public TValue? Value =>
        _value;

    public static implicit operator Result<TValue>(TValue value) => Create<TValue>(value);

}
public class Result
{
#pragma warning disable CS0169 // The field 'Result._value' is never used
    private Entity<ValueObject> _value;
#pragma warning restore CS0169 // The field 'Result._value' is never used
    protected Result() { }
    protected internal Result(bool isSuccess, Error error) {
        if (isSuccess && error != Error.None) throw new InvalidOperationException();
        if (!isSuccess && error == Error.None) throw new InvalidOperationException();
        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value)
        => new(value,true,Error.None);

    public static Result<TValue> Success<TValue>() where TValue: class
        => new(null, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) where TValue: class
        => new(null,false, error);

    public static Result Failure(Error error)
        => new(false, error);
    protected static Result<TValue> Create<TValue>(TValue value)
        => Success(value);

    public static Result<TValue> Combine<TValue>(params Result[] results) where TValue: class
    {
        var failures = results.Where(r => r.IsFailure).ToList();

        if (failures.Count == 0)
        {
            return Success<TValue>();
        }

        var combinedError = new Error("combined error", string.Join(", ", failures.Select(f => f.Error?.Message)));
        return Failure<TValue>(combinedError);
    }
}
