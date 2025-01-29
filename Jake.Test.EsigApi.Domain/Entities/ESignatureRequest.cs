using System.ComponentModel.DataAnnotations;
using Jake.Test.EsigApi.Domain.Enums;

namespace Jake.Test.EsigApi.Domain.Entities;

/// <summary>
/// Represents an electronic signature request
/// </summary>
public class ESignatureRequest
{
    /// <summary>
    /// The unique identifier for the e-signature request
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// The name of the document to be signed
    /// </summary>
    public string DocumentName { get; private set; }
    
    /// <summary>
    /// The content of the document to be signed (base64 encoded)
    /// </summary>
    public string DocumentContent { get; private set; }
    
    /// <summary>
    /// The email address of the signer
    /// </summary>
    public string SignerEmail { get; private set; }
    
    /// <summary>
    /// The name of the signer
    /// </summary>
    public string SignerName { get; private set; }
    
    /// <summary>
    /// Optional message to include in the signature request email
    /// </summary>
    public string? Message { get; private set; }
    
    /// <summary>
    /// The date and time when the request was created
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    
    /// <summary>
    /// The current status of the signature request
    /// </summary>
    public ESignatureStatus Status { get; private set; }

    private ESignatureRequest() { }

    public static ESignatureRequest Create(
        string documentName,
        string documentContent,
        string signerEmail,
        string signerName,
        string? message = null)
    {
        if (string.IsNullOrWhiteSpace(documentName))
            throw new ArgumentException("Document name is required", nameof(documentName));
            
        if (string.IsNullOrWhiteSpace(documentContent))
            throw new ArgumentException("Document content is required", nameof(documentContent));
            
        if (string.IsNullOrWhiteSpace(signerEmail))
            throw new ArgumentException("Signer email is required", nameof(signerEmail));
            
        if (string.IsNullOrWhiteSpace(signerName))
            throw new ArgumentException("Signer name is required", nameof(signerName));

        return new ESignatureRequest
        {
            Id = Guid.NewGuid(),
            DocumentName = documentName,
            DocumentContent = documentContent,
            SignerEmail = signerEmail,
            SignerName = signerName,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            Status = ESignatureStatus.Draft
        };
    }

    public void Send()
    {
        if (Status != ESignatureStatus.Draft && Status != ESignatureStatus.Expired)
            throw new InvalidOperationException($"Cannot send request in {Status} status");

        Status = ESignatureStatus.Sent;
    }

    public void Cancel()
    {
        if (Status == ESignatureStatus.Signed || Status == ESignatureStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel request in {Status} status");

        Status = ESignatureStatus.Cancelled;
    }

    public void MarkAsSigned()
    {
        if (Status != ESignatureStatus.Sent)
            throw new InvalidOperationException($"Cannot mark as signed when status is {Status}");

        Status = ESignatureStatus.Signed;
    }

    public void MarkAsExpired()
    {
        if (Status != ESignatureStatus.Sent)
            throw new InvalidOperationException($"Cannot mark as expired when status is {Status}");

        Status = ESignatureStatus.Expired;
    }
} 