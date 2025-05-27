using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.DTOs.User;

namespace QuestionBank.Application.Contracts.Services;

public interface IUserService
{
    Task<UserDto?> Add(AddUserDto dto);
    Task<UserDto?> Update(int id, UpdateUserDto dto);
    Task<PaginationDto<UserDto>> Search(SearchUserDto dto);
    Task<UserDto?> GetById(int id);
    Task<List<UserDto>> GetAll();
}