using QuestionBank.Application.DTOs.Auth;

namespace QuestionBank.Application.Contracts.Services;

public interface IAuthService
{
    Task<TokenDto?> Login(LoginDto dto);
}