using Microsoft.AspNetCore.Mvc;
using QuestionBank.Api.Responses;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Option;
using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.Notifications;
using Swashbuckle.AspNetCore.Annotations;

namespace QuestionBank.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class OptionController : BaseController
{
    private readonly IOptionService _optionService;

    public OptionController(INotificator notificator, IOptionService optionService) : base(notificator)
    {
        _optionService = optionService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add a new option", Tags = new[] { "Options" })]
    [ProducesResponseType(typeof(OptionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Add([FromBody] AddOptionDto dto)
    {
        var option = await _optionService.Add(dto);
        return CreatedResponse("", option);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a option", Tags = new[] { "Options" })]
    [ProducesResponseType(typeof(OptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOptionDto dto)
    {
        var option = await _optionService.Update(id, dto);
        return OkResponse(option);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a option", Tags = new[] { "Options" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _optionService.Delete(id);
        return NoContentResponse();
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search by options", Tags = new[] { "Options" })]
    [ProducesResponseType(typeof(PaginationDto<OptionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Search([FromQuery] SearchOptionDto dto)
    {
        var options = await _optionService.Search(dto);
        return OkResponse(options);
    }

    [HttpGet("get-by-id/{id}")]
    [SwaggerOperation(Summary = "Get a option by ID", Tags = new[] { "Options" })]
    [ProducesResponseType(typeof(OptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var option = await _optionService.GetById(id);
        return OkResponse(option);
    }

    [HttpGet("get-all")]
    [SwaggerOperation(Summary = "Get all options", Tags = new[] { "Options" })]
    [ProducesResponseType(typeof(List<OptionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var options = await _optionService.GetAll();
        return OkResponse(options);
    }
}