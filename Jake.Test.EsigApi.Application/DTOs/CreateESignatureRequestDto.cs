using System.ComponentModel.DataAnnotations;

namespace Jake.Test.EsigApi.Application.DTOs;

public class CreateESignatureRequestDto
{
    [Required]
    public string DocumentName { get; set; } = string.Empty;
    
    [Required]
    public string DocumentContent { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string SignerEmail { get; set; } = string.Empty;
    
    [Required]
    public string SignerName { get; set; } = string.Empty;
    
    public string? Message { get; set; }
} 