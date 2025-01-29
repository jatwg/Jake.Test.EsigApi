namespace Jake.Test.EsigApi.API.Models;

/// <summary>
/// Represents a standardized API error response
/// </summary>
public class ApiErrorResponse
{
    public string Message { get; set; }
    public string[] Errors { get; set; }
    public string TraceId { get; set; }
} 