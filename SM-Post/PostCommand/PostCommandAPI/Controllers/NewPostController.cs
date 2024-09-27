using API.Commands;
using API.DTOs;
using CQRSCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using PostCommon.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/vi/[controller]")]
public class NewPostController: ControllerBase
{
    private readonly ILogger<NewPostController> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public NewPostController(ILogger<NewPostController> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
    }
    
    [HttpPost]
    public async Task<ActionResult> NewPostAsync(NewPostCommand command)
    {
        var id = Guid.NewGuid();
        try
        {
            command.Id = id;
        
            await _commandDispatcher.SendAsync(command);

            return StatusCode(StatusCodes.Status201Created, new NewPostResponse
            {
                Message = "New post creation request completed successfully"
            });
        }
        catch (InvalidOperationException e)
        {
            _logger.Log(LogLevel.Warning, e, "Client made a bad request!");
            return BadRequest(new BaseResponse
            {
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            const string SAFE_ERROR_MESSAGE = "An error occurred while request create a new post";
            _logger.Log(LogLevel.Warning, e, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse()
            {
                Id = id,
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }
}