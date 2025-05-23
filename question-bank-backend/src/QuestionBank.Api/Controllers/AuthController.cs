using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestionBank.Api.Responses;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Auth;
using QuestionBank.Application.DTOs.User;
using QuestionBank.Application.Notifications;
using Swashbuckle.AspNetCore.Annotations;

namespace QuestionBank.Api.Controllers;

[AllowAnonymous]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(INotificator notificator, IAuthService authService) : base(notificator)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Authenticate", Tags = new[] { "Authentication" })]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _authService.Login(dto);
        return token != null ? OkResponse(token) : Unauthorized(new[] { "Incorrect email and/or password" });
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Register a user", Tags = new[] { "Authentication" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] AddUserDto dto)
    {
        var user = await _authService.Register(dto);
        return CreatedResponse("", user);
    }
}