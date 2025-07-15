using CertMailer.ExcelParser.Application.Interfaces;

namespace CertMailer.ExcelParser.Application.Models;

public class Result : IResult
{
    public IEnumerable<string> Errors { get; }
    public bool Success { get; }

    internal Result()
    {
        Success = true;
        Errors = ArraySegment<string>.Empty;
    }

    internal Result(IEnumerable<string> errors)
    {
        Success = false;
        Errors = errors;
    }

    public static Result Ok() => new();
    public static Result Fail(IEnumerable<string> errors) => new(errors);
}

public class Result<T> : Result, IResult<T>
{
    public T? Data { get; }
    
    internal Result(T data)
    {
        Data = data;
    }
    
    internal Result(IEnumerable<string> errors) : base(errors)
    {
    }

    public static Result<T> Ok(T data) => new(data);
    public new static Result<T> Fail(IEnumerable<string> errors) => new(errors);
}