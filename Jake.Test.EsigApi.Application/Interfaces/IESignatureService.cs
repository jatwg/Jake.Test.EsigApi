using Jake.Test.EsigApi.Application.DTOs;
using Jake.Test.EsigApi.Domain.Enums;

namespace Jake.Test.EsigApi.Application.Interfaces;

public interface IESignatureService
{
    Task<ESignatureRequestDto> CreateAsync(CreateESignatureRequestDto request);
    Task<ESignatureRequestDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ESignatureRequestDto>> GetAllAsync();
    Task<bool> SendAsync(Guid id);
    Task<bool> ResendAsync(Guid id);
    Task<bool> CancelAsync(Guid id);
    Task<ESignatureStatus> GetStatusAsync(Guid id);
    Task<byte[]?> DownloadAsync(Guid id);
} 