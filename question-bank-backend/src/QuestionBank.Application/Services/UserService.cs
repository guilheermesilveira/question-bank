using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QuestionBank.Application.Contracts.Services;
using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.DTOs.User;
using QuestionBank.Application.Notifications;
using QuestionBank.Domain.Entities;
using QuestionBank.Domain.Validators;
using QuestionBank.Infra.Contracts.Repositories;

namespace QuestionBank.Application.Services;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(
        INotificator notificator,
        IMapper mapper,
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher
    ) : base(notificator, mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto?> Add(AddUserDto dto)
    {
        if (!await ValidationsToAdd(dto))
            return null;

        var user = Mapper.Map<User>(dto);
        user.Password = _passwordHasher.HashPassword(user, dto.Password);
        user.IsAdmin = false;
        _userRepository.Add(user);

        return await CommitChanges() ? Mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto?> Update(int id, UpdateUserDto dto)
    {
        if (!await ValidationsToUpdate(id, dto))
            return null;

        var user = await _userRepository.FirstOrDefault(u => u.Id == id);
        user!.Name = dto.Name;
        user.Email = dto.Email;
        user.Password = dto.Password;
        user.Password = _passwordHasher.HashPassword(user, dto.Password);
        _userRepository.Update(user);

        return await CommitChanges() ? Mapper.Map<UserDto>(user) : null;
    }

    public async Task Delete(int id)
    {
        var user = await _userRepository.GetById(id);
        if (user == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        _userRepository.Delete(user);
        await CommitChanges();
    }

    public async Task<PaginationDto<UserDto>> Search(SearchUserDto dto)
    {
        var result = await _userRepository.Search(dto.Name, dto.Email, dto.NumberOfItemsPerPage,
            dto.CurrentPage);

        return new PaginationDto<UserDto>
        {
            TotalItems = result.TotalItems,
            NumberOfItemsPerPage = result.NumberOfItemsPerPage,
            NumberOfPages = result.NumberOfPages,
            CurrentPage = result.CurrentPage,
            Items = Mapper.Map<List<UserDto>>(result.Items)
        };
    }

    public async Task<UserDto?> GetById(int id)
    {
        var user = await _userRepository.GetById(id);
        if (user != null)
            return Mapper.Map<UserDto>(user);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<List<UserDto>> GetAll()
    {
        var users = await _userRepository.GetAll();
        return Mapper.Map<List<UserDto>>(users);
    }

    private async Task<bool> ValidationsToAdd(AddUserDto dto)
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

    private async Task<bool> ValidationsToUpdate(int id, UpdateUserDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("The ID provided in the URL must be the same as the ID provided in the JSON");
            return false;
        }

        var userExist = await _userRepository.FirstOrDefault(u => u.Id == id);
        if (userExist == null)
        {
            Notificator.HandleNotFoundResource();
            return false;
        }

        var user = Mapper.Map<User>(dto);
        var validator = new UserValidator();

        var result = await validator.ValidateAsync(user);
        if (!result.IsValid)
        {
            Notificator.Handle(result.Errors);
            return false;
        }

        return true;
    }

    private async Task<bool> CommitChanges()
    {
        if (await _userRepository.UnitOfWork.Commit())
            return true;

        Notificator.Handle("An error occurred while saving changes");
        return false;
    }
}