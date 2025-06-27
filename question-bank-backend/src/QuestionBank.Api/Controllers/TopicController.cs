using Microsoft.AspNetCore.Mvc;
using QuestionBank.Api.Responses;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.DTOs.Topic;
using QuestionBank.Application.Notifications;
using Swashbuckle.AspNetCore.Annotations;

namespace QuestionBank.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class TopicController : BaseController
{
    private readonly ITopicService _topicService;

    public TopicController(INotificator notificator, ITopicService topicService) : base(notificator)
    {
        _topicService = topicService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add a new topic", Tags = new[] { "Topics" })]
    [ProducesResponseType(typeof(TopicDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Add([FromBody] AddTopicDto dto)
    {
        var topic = await _topicService.Add(dto);
        return CreatedResponse("", topic);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a topic", Tags = new[] { "Topics" })]
    [ProducesResponseType(typeof(TopicDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTopicDto dto)
    {
        var topic = await _topicService.Update(id, dto);
        return OkResponse(topic);
    }
    
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a topic", Tags = new[] { "Topics" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _topicService.Delete(id);
        return NoContentResponse();
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search by topics", Tags = new[] { "Topics" })]
    [ProducesResponseType(typeof(PaginationDto<TopicDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Search([FromQuery] SearchTopicDto dto)
    {
        var topics = await _topicService.Search(dto);
        return OkResponse(topics);
    }

    [HttpGet("get-by-id/{id}")]
    [SwaggerOperation(Summary = "Get a topic by ID", Tags = new[] { "Topics" })]
    [ProducesResponseType(typeof(TopicDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var topic = await _topicService.GetById(id);
        return OkResponse(topic);
    }

    [HttpGet("get-all")]
    [SwaggerOperation(Summary = "Get all topics", Tags = new[] { "Topics" })]
    [ProducesResponseType(typeof(List<TopicDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var topics = await _topicService.GetAll();
        return OkResponse(topics);
    }
}