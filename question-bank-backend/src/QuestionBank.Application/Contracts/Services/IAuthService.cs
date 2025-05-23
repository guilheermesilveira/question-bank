using QuestionBank.Application.DTOs.Auth;
using QuestionBank.Application.DTOs.User;

namespace QuestionBank.Application.Contracts.Services;

public interface IAuthService
{
    Task<TokenDto?> Login(LoginDto dto);
    Task<UserDto?> Register(AddUserDto dto);
}