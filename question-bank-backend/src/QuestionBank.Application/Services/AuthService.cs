using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using QuestionBank.Application.Configurations;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Auth;
using QuestionBank.Application.DTOs.User;
using QuestionBank.Application.Notifications;
using QuestionBank.Domain.Entities;
using QuestionBank.Domain.Validators;
using QuestionBank.Infra.Contracts.Repositories;

namespace QuestionBank.Application.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        INotificator notificator,
        IMapper mapper,
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtSettings
    ) : base(notificator, mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<TokenDto?> Login(LoginDto dto)
    {
        if (!await LoginValidations(dto))
            return null;

        var user = await _userRepository.FirstOrDefault(u => u.Email == dto.Email);
        if (user != null)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (result != PasswordVerificationResult.Failed)
            {
                return new TokenDto
                {
                    Token = await GenerateToken(user)
                };
            }
        }

        Notificator.Handle("Unable to login");
        return null;
    }

    public async Task<UserDto?> Register(AddUserDto dto)
    {
        if (!await ValidationsToRegister(dto))
            return null;

        var user = Mapper.Map<User>(dto);
        user.Password = _passwordHasher.HashPassword(user, dto.Password);
        user.IsAdmin = false;
        _userRepository.Add(user);

        return await CommitChanges() ? Mapper.Map<UserDto>(user) : null;
    }

    private async Task<bool> LoginValidations(LoginDto dto)
    {
        var user = Mapper.Map<User>(dto);
        var validator = new LoginValidator();

        var result = await validator.ValidateAsync(user);
        if (!result.IsValid)
        {
            Notificator.Handle(result.Errors);
            return false;
        }

        var userExist = await _userRepository.FirstOrDefault(u => u.Email == dto.Email);
        if (userExist == null)
        {
            Notificator.HandleNotFoundResource();
            return false;
        }

        return true;
    }

    private async Task<bool> ValidationsToRegister(AddUserDto dto)
    {
        var user = Mapper.Map<User>(dto);
        var validator = new UserValidator();

        var result = await validator.ValidateAsync(user);
        if (!result.IsValid)
        {
            Notificator.Handle(result.Errors);
            return false;
        }

        var emailExist = await _userRepository.FirstOrDefault(u => u.Email == dto.Email);
        if (emailExist != null)
        {
            Notificator.Handle("Email already used by another user");
            return false;
        }

        return true;
    }

    private async Task<string> GenerateToken(User user)
    {
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        claimsIdentity.AddClaim(user.IsAdmin
            ? new Claim(ClaimTypes.Role, "Admin")
            : new Claim(ClaimTypes.Role, "User"));

        var key = await _jwtService.GetCurrentSigningCredentials();
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.HoursUntilExpiry),
            SigningCredentials = key
        });

        return tokenHandler.WriteToken(token);
    }

    private async Task<bool> CommitChanges()
    {
        if (await _userRepository.UnitOfWork.Commit())
            return true;

        Notificator.Handle("An error occurred while saving changes");
        return false;
    }
}