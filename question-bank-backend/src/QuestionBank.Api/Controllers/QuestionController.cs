using Microsoft.AspNetCore.Mvc;
using QuestionBank.Api.Responses;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.DTOs.Question;
using QuestionBank.Application.Notifications;
using Swashbuckle.AspNetCore.Annotations;

namespace QuestionBank.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class QuestionController : BaseController
{
    private readonly IQuestionService _questionService;

    public QuestionController(INotificator notificator, IQuestionService questionService) : base(notificator)
    {
        _questionService = questionService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add a new question", Tags = new[] { "Questions" })]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Add([FromBody] AddQuestionDto dto)
    {
        var question = await _questionService.Add(dto);
        return CreatedResponse("", question);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a question", Tags = new[] { "Questions" })]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateQuestionDto dto)
    {
        var question = await _questionService.Update(id, dto);
        return OkResponse(question);
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search by questions", Tags = new[] { "Questions" })]
    [ProducesResponseType(typeof(PaginationDto<QuestionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Search([FromQuery] SearchQuestionDto dto)
    {
        var questions = await _questionService.Search(dto);
        return OkResponse(questions);
    }

    [HttpGet("get-by-id/{id}")]
    [SwaggerOperation(Summary = "Get a question by ID", Tags = new[] { "Questions" })]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var question = await _questionService.GetById(id);
        return OkResponse(question);
    }

    [HttpGet("get-all")]
    [SwaggerOperation(Summary = "Get all questions", Tags = new[] { "Questions" })]
    [ProducesResponseType(typeof(List<QuestionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var questions = await _questionService.GetAll();
        return OkResponse(questions);
    }
}