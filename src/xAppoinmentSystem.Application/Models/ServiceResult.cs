namespace xAppoinmentSystem.Application.Models;

public class ServiceResult
{
    
}

public sealed class ServiceResult<T> : ServiceResult
{
    private ServiceResult(T? data)
    {
        Data = data;
    }
    
    public T? Data { get; }

    public static ServiceResult<T> From(T data)
    {
        return new ServiceResult<T>(data);
    }

    public static implicit operator ServiceResult<T>(T? data)
    {
        return new ServiceResult<T>(data);
    }
}

public sealed class ServiceResultMessage : ServiceResult
{
    private ServiceResultMessage(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);

        Message = message;
    }
    
    public string Message { get;}

    public static ServiceResultMessage From(string message)
    {
        return new ServiceResultMessage(message);
    }
}