using Jake.Test.EsigApi.Application.DTOs;
using Jake.Test.EsigApi.Application.Features.ESignature.Commands.CreateESignatureRequest;
using Jake.Test.EsigApi.Application.Features.ESignature.Commands.SendESignatureRequest;
using Jake.Test.EsigApi.Application.Features.ESignature.Commands.ResendESignatureRequest;
using Jake.Test.EsigApi.Application.Features.ESignature.Commands.CancelESignatureRequest;
using Jake.Test.EsigApi.Application.Features.ESignature.Queries.GetAllESignatureRequests;
using Jake.Test.EsigApi.Application.Features.ESignature.Queries.GetESignatureRequestById;
using Jake.Test.EsigApi.Application.Features.ESignature.Queries.GetESignatureRequestStatus;
using Jake.Test.EsigApi.Application.Features.ESignature.Queries.DownloadESignatureRequest;
using Microsoft.AspNetCore.Mvc;
using Jake.Test.EsigApi.Domain.Enums;
using Jake.Test.EsigApi.Application.Exceptions;
using Jake.Test.EsigApi.Application.Common.Results;
using MediatR;

namespace Jake.Test.EsigApi.API.Controllers;

/// <summary>
/// Controller for managing electronic signature requests
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class ESignatureController : BaseApiController<ESignatureController>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the ESignatureController
    /// </summary>
    public ESignatureController(IMediator mediator, ILogger<ESignatureController> logger)
        : base(logger)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new e-signature request
    /// </summary>
    /// <param name="request">The e-signature request details</param>
    /// <returns>The created e-signature request</returns>
    /// <response code="201">Returns the newly created e-signature request</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(ESignatureRequestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ESignatureRequestDto>> Create([FromBody] CreateESignatureRequestDto requestDto)
    {
        if (requestDto == null)
        {
            Logger.LogWarning("Create request was null");
            return BadRequest("Request cannot be null");
        }

        try
        {
            Logger.LogInformation("Creating new e-signature request for document: {DocumentName}, Signer: {SignerEmail}", 
                requestDto.DocumentName, requestDto.SignerEmail);
            
            var command = new CreateESignatureRequestCommand
            {
                DocumentName = requestDto.DocumentName,
                DocumentContent = requestDto.DocumentContent,
                SignerEmail = requestDto.SignerEmail,
                SignerName = requestDto.SignerName,
                Message = requestDto.Message
            };
            
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                Logger.LogWarning("Failed to create e-signature request: {Error}", result.Error);
                return result.IsNotFound ? NotFound(result.Error) : BadRequest(result.Error);
            }
            
            Logger.LogInformation("Successfully created e-signature request with ID: {RequestId}", result.Value.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }
        catch (ValidationException ex)
        {
            Logger.LogWarning(ex, "Validation failed for e-signature request");
            return BadRequest(new ValidationProblemDetails(ex.Errors));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error creating e-signature request for document: {DocumentName}", requestDto.DocumentName);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all e-signature requests
    /// </summary>
    /// <returns>A list of all e-signature requests</returns>
    /// <response code="200">Returns the list of e-signature requests</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ESignatureRequestDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ESignatureRequestDto>>> GetAll()
    {
        try
        {
            Logger.LogInformation("Retrieving all e-signature requests");
            
            var query = new GetAllESignatureRequestsQuery();
            var result = await _mediator.Send(query);
            
            Logger.LogInformation("Retrieved {Count} e-signature requests", result.Value.Count());
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error retrieving all e-signature requests");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a specific e-signature request by ID
    /// </summary>
    /// <param name="id">The ID of the e-signature request</param>
    /// <returns>The requested e-signature request</returns>
    /// <response code="200">Returns the requested e-signature request</response>
    /// <response code="404">If the e-signature request is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ESignatureRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ESignatureRequestDto>> GetById(Guid id)
    {
        try
        {
            Logger.LogInformation("Retrieving e-signature request with ID: {RequestId}", id);
            
            var query = new GetESignatureRequestByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (!result.IsSuccess)
            {
                Logger.LogWarning("Failed to retrieve e-signature request: {Error}", result.Error);
                return result.IsNotFound ? NotFound(result.Error) : BadRequest(result.Error);
            }
            
            Logger.LogInformation("Successfully retrieved e-signature request with ID: {RequestId}", id);
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error retrieving e-signature request with ID: {RequestId}", id);
            throw;
        }
    }

    /// <summary>
    /// Sends an e-signature request to the signer
    /// </summary>
    /// <param name="id">The ID of the e-signature request to send</param>
    /// <returns>No content if successful</returns>
    /// <response code="200">If the request was sent successfully</response>
    /// <response code="404">If the e-signature request is not found</response>
    [HttpPost("{id}/send")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Send(Guid id)
    {
        try
        {
            Logger.LogInformation("Sending e-signature request with ID: {RequestId}", id);
            
            var command = new SendESignatureRequestCommand(id);
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
            {
                Logger.LogWarning("Failed to send e-signature request: {Error}", result.Error);
                return result.IsNotFound ? NotFound(result.Error) : BadRequest(result.Error);
            }
            
            Logger.LogInformation("Successfully sent e-signature request with ID: {RequestId}", id);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error sending e-signature request with ID: {RequestId}", id);
            throw;
        }
    }

    /// <summary>
    /// Resends an e-signature request to the signer
    /// </summary>
    /// <param name="id">The ID of the e-signature request to resend</param>
    /// <returns>No content if successful</returns>
    /// <response code="200">If the request was resent successfully</response>
    /// <response code="404">If the e-signature request is not found or cannot be resent</response>
    [HttpPost("{id}/resend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Resend(Guid id)
    {
        try
        {
            Logger.LogInformation("Resending e-signature request with ID: {RequestId}", id);
            
            var command = new ResendESignatureRequestCommand(id);
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
            {
                Logger.LogWarning("Failed to resend e-signature request: {Error}", result.Error);
                return result.IsNotFound ? NotFound(result.Error) : BadRequest(result.Error);
            }
            
            Logger.LogInformation("Successfully resent e-signature request with ID: {RequestId}", id);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error resending e-signature request with ID: {RequestId}", id);
            throw;
        }
    }

    /// <summary>
    /// Gets the current status of an e-signature request
    /// </summary>
    /// <param name="id">The ID of the e-signature request</param>
    /// <returns>The current status of the request</returns>
    /// <response code="200">Returns the current status</response>
    /// <response code="404">If the e-signature request is not found</response>
    [HttpGet("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ESignatureStatus>> GetStatus(Guid id)
    {
        try
        {
            Logger.LogInformation("Retrieving status for e-signature request with ID: {RequestId}", id);
            
            var query = new GetESignatureRequestStatusQuery(id);
            var result = await _mediator.Send(query);
            
            if (!result.IsSuccess)
            {
                Logger.LogWarning("Failed to retrieve status: {Error}", result.Error);
                return result.IsNotFound ? NotFound(result.Error) : BadRequest(result.Error);
            }
            
            Logger.LogInformation("Status for e-signature request {RequestId} is: {Status}", id, result.Value);
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error retrieving status for e-signature request with ID: {RequestId}", id);
            throw;
        }
    }

    /// <summary>
    /// Downloads the signed document
    /// </summary>
    /// <param name="id">The ID of the e-signature request</param>
    /// <returns>The signed document as a PDF file</returns>
    /// <response code="200">Returns the signed document</response>
    /// <response code="404">If the e-signature request or document is not found</response>
    [HttpGet("{id}/download")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(Guid id)
    {
        try
        {
            Logger.LogInformation("Downloading signed document for e-signature request with ID: {RequestId}", id);
            
            var query = new DownloadESignatureRequestQuery(id);
            var result = await _mediator.Send(query);
            
            if (!result.IsSuccess)
            {
                Logger.LogWarning("Failed to download document: {Error}", result.Error);
                return result.IsNotFound ? NotFound(result.Error) : BadRequest(result.Error);
            }
            
            Logger.LogInformation("Successfully downloaded signed document for e-signature request with ID: {RequestId}", id);
            return File(result.Value, "application/pdf", $"document_{id}.pdf");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error downloading document for e-signature request with ID: {RequestId}", id);
            throw;
        }
    }

    /// <summary>
    /// Cancels an e-signature request
    /// </summary>
    /// <param name="id">The ID of the e-signature request to cancel</param>
    /// <returns>No content if successful</returns>
    /// <response code="200">If the request was cancelled successfully</response>
    /// <response code="404">If the e-signature request is not found</response>
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(Guid id)
    {
        try
        {
            Logger.LogInformation("Cancelling e-signature request with ID: {RequestId}", id);
            
            var command = new CancelESignatureRequestCommand(id);
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
            {
                Logger.LogWarning("Failed to cancel e-signature request: {Error}", result.Error);
                return result.IsNotFound ? NotFound(result.Error) : BadRequest(result.Error);
            }
            
            Logger.LogInformation("Successfully cancelled e-signature request with ID: {RequestId}", id);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error cancelling e-signature request with ID: {RequestId}", id);
            throw;
        }
    }
} 