namespace CertMailer.Application.Interfaces;

public interface IResult
{
    IEnumerable<string> Errors { get; }
    bool Success { get; }
}

public interface IResult<out T> : IResult
{
    T? Data { get; }
}