
using Jake.Test.EsigApi.Application.Interfaces;
using Jake.Test.EsigApi.Domain.Entities;

namespace Jake.Test.EsigApi.Infrastructure.Persistence;

public class InMemoryESignatureRepository : IESignatureRepository
{
    private readonly Dictionary<Guid, ESignatureRequest> _signatures = new();
    private readonly Dictionary<Guid, byte[]> _documents = new();

    public Task<ESignatureRequest> CreateAsync(ESignatureRequest request)
    {
        _signatures[request.Id] = request;
        _documents[request.Id] = Convert.FromBase64String(request.DocumentContent);
        return Task.FromResult(request);
    }

    public Task<ESignatureRequest?> GetByIdAsync(Guid id)
    {
        _signatures.TryGetValue(id, out var request);
        return Task.FromResult(request);
    }

    public Task<IEnumerable<ESignatureRequest>> GetAllAsync()
    {
        return Task.FromResult(_signatures.Values.AsEnumerable());
    }

    public Task<bool> UpdateAsync(ESignatureRequest request)
    {
        if (!_signatures.ContainsKey(request.Id))
            return Task.FromResult(false);

        _signatures[request.Id] = request;
        return Task.FromResult(true);
    }

    public Task<byte[]?> GetDocumentAsync(Guid id)
    {
        _documents.TryGetValue(id, out var document);
        return Task.FromResult(document);
    }
} 