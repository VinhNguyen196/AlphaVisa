using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaVisa.Domain.Enums;

namespace AlphaVisa.SharedKernel;

public sealed record Error
{
    public int Code { get; set; }

    public string? Description { get; set; }

    public int Type { get; set; }

    private Error(ErrorCodes errorCode, string description, ErrorTypes errorType)
    {
        Code = (int)errorCode;
        Description = description;
        Type = (int)errorType;
    }

    public static readonly Error None = new(ErrorCodes.None, string.Empty, ErrorTypes.None);

    public static readonly Error NullValue = new(ErrorCodes.NullValue, "Null value was provided", ErrorTypes.Validation);
}

public class Result
{
    public bool IsSuccess { get; set; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of an error result can't be accessed.");

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

}
