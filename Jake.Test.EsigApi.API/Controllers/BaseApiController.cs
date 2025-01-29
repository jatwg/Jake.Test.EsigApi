using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Jake.Test.EsigApi.API.Controllers;

/// <summary>
/// Base API controller with common functionality
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public abstract class BaseApiController<T> : ControllerBase where T : class
{
    protected readonly ILogger<T> Logger;

    protected BaseApiController(ILogger<T> logger)
    {
        Logger = logger;
    }

    protected ActionResult<TResult> NotFoundWithLog<TResult>(string message, params object[] args)
    {
        Logger.LogWarning(message, args);
        return NotFound();
    }

    protected ActionResult<TResult> BadRequestWithLog<TResult>(string message, params object[] args)
    {
        Logger.LogWarning(message, args);
        return BadRequest(message);
    }

    protected IActionResult InternalServerErrorWithLog(Exception ex, string message, params object[] args)
    {
        Logger.LogError(ex, message, args);
        return StatusCode(StatusCodes.Status500InternalServerError, 
            new ProblemDetails 
            { 
                Title = "An error occurred while processing your request.",
                Status = StatusCodes.Status500InternalServerError
            });
    }
} 
