using Microsoft.AspNetCore.Mvc;
using QuestionBank.Api.Responses;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.DTOs.User;
using QuestionBank.Application.Notifications;
using Swashbuckle.AspNetCore.Annotations;

namespace QuestionBank.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(INotificator notificator, IUserService userService) : base(notificator)
    {
        _userService = userService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add a new user", Tags = new[] { "Users" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Add([FromBody] AddUserDto dto)
    {
        var user = await _userService.Add(dto);
        return CreatedResponse("", user);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a user", Tags = new[] { "Users" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var user = await _userService.Update(id, dto);
        return OkResponse(user);
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search by users", Tags = new[] { "Users" })]
    [ProducesResponseType(typeof(PaginationDto<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Search([FromQuery] SearchUserDto dto)
    {
        var users = await _userService.Search(dto);
        return OkResponse(users);
    }

    [HttpGet("get-all")]
    [SwaggerOperation(Summary = "Get all users", Tags = new[] { "Users" })]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAll();
        return OkResponse(users);
    }
}