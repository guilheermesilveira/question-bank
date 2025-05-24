using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestionBank.Api.Responses;
using QuestionBank.Application.Notifications;

namespace QuestionBank.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
public abstract class BaseController : Controller
{
    private readonly INotificator _notificator;

    protected BaseController(INotificator notificator)
    {
        _notificator = notificator;
    }

    protected IActionResult CreatedResponse(string uri = "", object? result = null)
    {
        return CustomResponse(Created(uri, result));
    }

    protected IActionResult OkResponse(object? result = null)
    {
        return CustomResponse(Ok(result));
    }

    protected IActionResult NoContentResponse()
    {
        return CustomResponse(NoContent());
    }

    private IActionResult CustomResponse(IActionResult objectResult)
    {
        if (SuccessfulOperation)
            return objectResult;

        if (_notificator.IsNotFoundResource)
            return NotFound();

        var badRequestResponse = new BadRequestResponse(_notificator.GetNotifications().ToList());
        return BadRequest(badRequestResponse);
    }

    private bool SuccessfulOperation => !(_notificator.HasNotification || _notificator.IsNotFoundResource);
}