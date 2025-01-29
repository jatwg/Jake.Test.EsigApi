using Jake.Test.EsigApi.Application.DTOs;
using Jake.Test.EsigApi.Application.Interfaces;
using Jake.Test.EsigApi.Domain.Entities;
using Jake.Test.EsigApi.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Jake.Test.EsigApi.Application.Services;

public class ESignatureService : IESignatureService
{
    private readonly IESignatureRepository _repository;
    private readonly ILogger<ESignatureService> _logger;

    public ESignatureService(IESignatureRepository repository, ILogger<ESignatureService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ESignatureRequestDto> CreateAsync(CreateESignatureRequestDto request)
    {
        var entity = ESignatureRequest.Create(
            request.DocumentName,
            request.DocumentContent,
            request.SignerEmail,
            request.SignerName,
            request.Message);

        var result = await _repository.CreateAsync(entity);
        _logger.LogInformation("Created new e-signature request with ID: {Id}", result.Id);
        
        return ESignatureRequestDto.FromEntity(result);
    }

    public async Task<ESignatureRequestDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity != null ? ESignatureRequestDto.FromEntity(entity) : null;
    }

    public async Task<IEnumerable<ESignatureRequestDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(ESignatureRequestDto.FromEntity);
    }

    public async Task<bool> SendAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return false;

        try
        {
            entity.Send();
            var result = await _repository.UpdateAsync(entity);
            if (result)
            {
                _logger.LogInformation("Sent e-signature request {Id} to {Email}", id, entity.SignerEmail);
            }
            return result;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to send e-signature request {Id}", id);
            return false;
        }
    }

    public async Task<bool> ResendAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return false;

        try
        {
            entity.Send();
            var result = await _repository.UpdateAsync(entity);
            if (result)
            {
                _logger.LogInformation("Resent e-signature request {Id} to {Email}", id, entity.SignerEmail);
            }
            return result;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to resend e-signature request {Id}", id);
            return false;
        }
    }

    public async Task<bool> CancelAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return false;

        try
        {
            entity.Cancel();
            var result = await _repository.UpdateAsync(entity);
            if (result)
            {
                _logger.LogInformation("Cancelled e-signature request {Id}", id);
            }
            return result;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to cancel e-signature request {Id}", id);
            return false;
        }
    }

    public async Task<ESignatureStatus> GetStatusAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"E-signature request with ID {id} not found");

        return entity.Status;
    }

    public async Task<byte[]?> DownloadAsync(Guid id)
    {
        return await _repository.GetDocumentAsync(id);
    }

    Task<ESignatureStatus> IESignatureService.GetStatusAsync(Guid id)
    {
        throw new NotImplementedException();
    }
} 