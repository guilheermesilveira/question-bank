using Microsoft.AspNetCore.Mvc;
using QuestionBank.Api.Responses;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.UserAnswer;
using QuestionBank.Application.Notifications;
using Swashbuckle.AspNetCore.Annotations;

namespace QuestionBank.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class UserAnswerController : BaseController
{
    private readonly IUserAnswerService _userAnswerService;

    public UserAnswerController(INotificator notificator, IUserAnswerService userAnswerService) : base(notificator)
    {
        _userAnswerService = userAnswerService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add a new user answer", Tags = new[] { "UserAnswers" })]
    [ProducesResponseType(typeof(UserAnswerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Add([FromBody] AddUserAnswerDto dto)
    {
        var userAnswer = await _userAnswerService.Add(dto);
        return CreatedResponse("", userAnswer);
    }

    [HttpGet("get-by-id/{id}")]
    [SwaggerOperation(Summary = "Get a user answer by ID", Tags = new[] { "UserAnswers" })]
    [ProducesResponseType(typeof(UserAnswerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var userAnswer = await _userAnswerService.GetById(id);
        return OkResponse(userAnswer);
    }

    [HttpGet("get-all")]
    [SwaggerOperation(Summary = "Get all user answers", Tags = new[] { "UserAnswers" })]
    [ProducesResponseType(typeof(List<UserAnswerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var userAnswers = await _userAnswerService.GetAll();
        return OkResponse(userAnswers);
    }
}