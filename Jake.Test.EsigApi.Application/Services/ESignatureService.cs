using Jake.Test.EsigApi.Application.DTOs;
using Jake.Test.EsigApi.Application.Interfaces;
using Jake.Test.EsigApi.Domain.Entities;
using Jake.Test.EsigApi.Domain.Enums;
using Microsoft.Extensions.Logging;
using Jake.Test.EsigApi.Application.Common.Results;

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

    public async Task<Result<ESignatureRequestDto>> CreateAsync(CreateESignatureRequestDto request)
    {
        try
        {
            var entity = ESignatureRequest.Create(
                request.DocumentName,
                request.DocumentContent,
                request.SignerEmail,
                request.SignerName,
                request.Message);

            var result = await _repository.CreateAsync(entity);
            _logger.LogInformation("Created new e-signature request with ID: {Id}", result.Id);
            
            return Result<ESignatureRequestDto>.Success(ESignatureRequestDto.FromEntity(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create e-signature request");
            return Result<ESignatureRequestDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<ESignatureRequestDto>> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            return Result<ESignatureRequestDto>.NotFound($"E-signature request with ID {id} not found");
        }
        
        return Result<ESignatureRequestDto>.Success(ESignatureRequestDto.FromEntity(entity));
    }

    public async Task<Result<IEnumerable<ESignatureRequestDto>>> GetAllAsync()
    {
        try
        {
            var entities = await _repository.GetAllAsync();
            var dtos = entities.Select(ESignatureRequestDto.FromEntity);
            return Result<IEnumerable<ESignatureRequestDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve e-signature requests");
            return Result<IEnumerable<ESignatureRequestDto>>.Failure(ex.Message);
        }
    }

    public async Task<Result> SendAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return Result.NotFound($"E-signature request with ID {id} not found");

        try
        {
            entity.Send();
            var result = await _repository.UpdateAsync(entity);
            if (result)
            {
                _logger.LogInformation("Sent e-signature request {Id} to {Email}", id, entity.SignerEmail);
                return Result.Success();
            }
            return Result.Failure("Failed to update e-signature request");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to send e-signature request {Id}", id);
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> ResendAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return Result.NotFound($"E-signature request with ID {id} not found");

        try
        {
            entity.Send();
            var result = await _repository.UpdateAsync(entity);
            if (result)
            {
                _logger.LogInformation("Resent e-signature request {Id} to {Email}", id, entity.SignerEmail);
                return Result.Success();
            }
            return Result.Failure("Failed to update e-signature request");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to resend e-signature request {Id}", id);
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result<ESignatureStatus>> GetStatusAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return Result<ESignatureStatus>.NotFound($"E-signature request with ID {id} not found");

        return Result<ESignatureStatus>.Success(entity.Status);
    }

    public async Task<Result<byte[]>> DownloadAsync(Guid id)
    {
        var document = await _repository.GetDocumentAsync(id);
        if (document == null)
            return Result<byte[]>.NotFound($"Document not found for e-signature request with ID {id}");

        return Result<byte[]>.Success(document);
    }

    public async Task<Result> CancelAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return Result.NotFound($"E-signature request with ID {id} not found");

        try
        {
            entity.Cancel();
            var result = await _repository.UpdateAsync(entity);
            if (result)
            {
                _logger.LogInformation("Cancelled e-signature request {Id}", id);
                return Result.Success();
            }
            return Result.Failure("Failed to update e-signature request");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to cancel e-signature request {Id}", id);
            return Result.Failure(ex.Message);
        }
    }
} 