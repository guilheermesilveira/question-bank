using Microsoft.AspNetCore.Mvc;
using QuestionBank.Api.Responses;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Test;
using QuestionBank.Application.Notifications;
using Swashbuckle.AspNetCore.Annotations;

namespace QuestionBank.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class TestController : BaseController
{
    private readonly ITestService _testService;

    public TestController(INotificator notificator, ITestService testService) : base(notificator)
    {
        _testService = testService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new test", Tags = new[] { "Tests" })]
    [ProducesResponseType(typeof(TestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateTestDto dto)
    {
        var test = await _testService.Create(dto);
        return CreatedResponse("", test);
    }

    [HttpPost("finish")]
    [SwaggerOperation(Summary = "Finish a test", Tags = new[] { "Tests" })]
    [ProducesResponseType(typeof(TestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Finish([FromBody] FinishTestDto dto)
    {
        var test = await _testService.Finish(dto);
        return CreatedResponse("", test);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a test by ID", Tags = new[] { "Tests" })]
    [ProducesResponseType(typeof(TestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var test = await _testService.GetById(id);
        return OkResponse(test);
    }
}