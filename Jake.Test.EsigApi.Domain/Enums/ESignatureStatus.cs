namespace Jake.Test.EsigApi.Domain.Enums;

/// <summary>
/// Represents the status of an e-signature request
/// </summary>
public enum ESignatureStatus
{
    /// <summary>
    /// The request has been created but not yet sent
    /// </summary>
    Draft,

    /// <summary>
    /// The request has been sent to the signer
    /// </summary>
    Sent,

    /// <summary>
    /// The document has been signed
    /// </summary>
    Signed,

    /// <summary>
    /// The signature request has expired
    /// </summary>
    Expired,

    /// <summary>
    /// The signature request has been cancelled
    /// </summary>
    Cancelled,

    /// <summary>
    /// The signature request has failed
    /// </summary>
    Failed
} 