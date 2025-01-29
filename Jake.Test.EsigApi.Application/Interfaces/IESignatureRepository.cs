using Jake.Test.EsigApi.Domain.Entities;

namespace Jake.Test.EsigApi.Application.Interfaces;

public interface IESignatureRepository
{
    Task<ESignatureRequest> CreateAsync(ESignatureRequest request);
    Task<ESignatureRequest?> GetByIdAsync(Guid id);
    Task<IEnumerable<ESignatureRequest>> GetAllAsync();
    Task<bool> UpdateAsync(ESignatureRequest request);
    Task<byte[]?> GetDocumentAsync(Guid id);
} 