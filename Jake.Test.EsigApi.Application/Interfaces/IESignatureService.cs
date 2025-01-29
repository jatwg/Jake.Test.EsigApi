using Jake.Test.EsigApi.Application.DTOs;
using Jake.Test.EsigApi.Domain.Enums;
using Jake.Test.EsigApi.Application.Common.Results;

namespace Jake.Test.EsigApi.Application.Interfaces;

public interface IESignatureService
{
    Task<Result<ESignatureRequestDto>> CreateAsync(CreateESignatureRequestDto request);
    Task<Result<IEnumerable<ESignatureRequestDto>>> GetAllAsync();
    Task<Result<ESignatureRequestDto>> GetByIdAsync(Guid id);
    Task<Result> SendAsync(Guid id);
    Task<Result> ResendAsync(Guid id);
    Task<Result<ESignatureStatus>> GetStatusAsync(Guid id);
    Task<Result<byte[]>> DownloadAsync(Guid id);
    Task<Result> CancelAsync(Guid id);
} 