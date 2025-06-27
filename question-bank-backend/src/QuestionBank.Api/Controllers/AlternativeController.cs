using Microsoft.AspNetCore.Mvc;
using QuestionBank.Api.Responses;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Alternative;
using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.Notifications;
using Swashbuckle.AspNetCore.Annotations;

namespace QuestionBank.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class AlternativeController : BaseController
{
    private readonly IAlternativeService _alternativeService;

    public AlternativeController(INotificator notificator, IAlternativeService alternativeService) : base(notificator)
    {
        _alternativeService = alternativeService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add a new alternative", Tags = new[] { "Alternatives" })]
    [ProducesResponseType(typeof(AlternativeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Add([FromBody] AddAlternativeDto dto)
    {
        var alternative = await _alternativeService.Add(dto);
        return CreatedResponse("", alternative);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a alternative", Tags = new[] { "Alternatives" })]
    [ProducesResponseType(typeof(AlternativeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAlternativeDto dto)
    {
        var alternative = await _alternativeService.Update(id, dto);
        return OkResponse(alternative);
    }
    
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a alternative", Tags = new[] { "Alternatives" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _alternativeService.Delete(id);
        return NoContentResponse();
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search by alternatives", Tags = new[] { "Alternatives" })]
    [ProducesResponseType(typeof(PaginationDto<AlternativeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Search([FromQuery] SearchAlternativeDto dto)
    {
        var alternatives = await _alternativeService.Search(dto);
        return OkResponse(alternatives);
    }

    [HttpGet("get-by-id/{id}")]
    [SwaggerOperation(Summary = "Get a alternative by ID", Tags = new[] { "Alternatives" })]
    [ProducesResponseType(typeof(AlternativeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var alternative = await _alternativeService.GetById(id);
        return OkResponse(alternative);
    }

    [HttpGet("get-all")]
    [SwaggerOperation(Summary = "Get all alternatives", Tags = new[] { "Alternatives" })]
    [ProducesResponseType(typeof(List<AlternativeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var alternatives = await _alternativeService.GetAll();
        return OkResponse(alternatives);
    }
}