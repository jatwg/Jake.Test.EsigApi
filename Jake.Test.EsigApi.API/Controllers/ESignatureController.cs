using Jake.Test.EsigApi.Application.DTOs;
using Jake.Test.EsigApi.Domain.Entities;
using Jake.Test.EsigApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Jake.Test.EsigApi.Domain.Enums;

namespace Jake.Test.EsigApi.API.Controllers;

/// <summary>
/// Controller for managing electronic signature requests
/// </summary>
[ApiController]
[Route("[controller]")]
public class ESignatureController : ControllerBase
{
    private readonly IESignatureService _signatureService;
    private readonly ILogger<ESignatureController> _logger;

    /// <summary>
    /// Initializes a new instance of the ESignatureController
    /// </summary>
    public ESignatureController(IESignatureService signatureService, ILogger<ESignatureController> logger)
    {
        _signatureService = signatureService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new e-signature request
    /// </summary>
    /// <param name="request">The e-signature request details</param>
    /// <returns>The created e-signature request</returns>
    /// <response code="201">Returns the newly created e-signature request</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ESignatureRequestDto>> Create([FromBody] CreateESignatureRequestDto request)
    {
        var result = await _signatureService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Retrieves all e-signature requests
    /// </summary>
    /// <returns>A list of all e-signature requests</returns>
    /// <response code="200">Returns the list of e-signature requests</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ESignatureRequestDto>>> GetAll()
    {
        var signatures = await _signatureService.GetAllAsync();
        return Ok(signatures);
    }

    /// <summary>
    /// Retrieves a specific e-signature request by ID
    /// </summary>
    /// <param name="id">The ID of the e-signature request</param>
    /// <returns>The requested e-signature request</returns>
    /// <response code="200">Returns the requested e-signature request</response>
    /// <response code="404">If the e-signature request is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ESignatureRequestDto>> GetById(Guid id)
    {
        var signature = await _signatureService.GetByIdAsync(id);
        if (signature == null)
        {
            return NotFound();
        }
        return Ok(signature);
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
        var result = await _signatureService.SendAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
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
        var result = await _signatureService.ResendAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
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
            var status = await _signatureService.GetStatusAsync(id);
            return Ok(status);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
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
        var document = await _signatureService.DownloadAsync(id);
        if (document == null)
        {
            return NotFound();
        }
        return File(document, "application/pdf", $"document_{id}.pdf");
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
        var result = await _signatureService.CancelAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }
} 