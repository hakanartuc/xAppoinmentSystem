using xAppoinmentSystem.Application.Models;

namespace xAppoinmentSystem.API.Models;

public class ApiResponse
{
    
}

public sealed class ApiResponse<T> : ApiResponse
{
    private ApiResponse(T data)
    {
        ArgumentNullException.ThrowIfNull(data);
        
        Data = data;
    }
    
    public T Data { get; }
    
    public static implicit operator ApiResponse<T>(ServiceResult<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(result.Data);
        
        return new ApiResponse<T>(result.Data);
    }

    public static ApiResponse<T> From(ServiceResult<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(result.Data);
        
        return new ApiResponse<T>(result.Data);
    }
}

public sealed class ApiResponseMessage : ApiResponse
{
    private ApiResponseMessage(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        
        Message = message;
    }
    
    public string Message { get; }
    
    public static implicit operator ApiResponseMessage(ServiceResultMessage result)
    {
        return new ApiResponseMessage(result.Message);
    }
}