using Jake.Test.EsigApi.Domain.Entities;
using Jake.Test.EsigApi.Domain.Enums;

namespace Jake.Test.EsigApi.Application.DTOs;

public class ESignatureRequestDto
{
    public Guid Id { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string SignerEmail { get; set; } = string.Empty;
    public string SignerName { get; set; } = string.Empty;
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public ESignatureStatus Status { get; set; }

    public static ESignatureRequestDto FromEntity(ESignatureRequest entity)
    {
        return new ESignatureRequestDto
        {
            Id = entity.Id,
            DocumentName = entity.DocumentName,
            SignerEmail = entity.SignerEmail,
            SignerName = entity.SignerName,
            Message = entity.Message,
            CreatedAt = entity.CreatedAt,
            Status = entity.Status
        };
    }
} 