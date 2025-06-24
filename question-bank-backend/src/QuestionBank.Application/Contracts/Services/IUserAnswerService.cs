using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.DTOs.UserAnswer;

namespace QuestionBank.Application.Contracts.Services;

public interface IUserAnswerService
{
    Task<UserAnswerDto?> Add(AddUserAnswerDto dto);
    Task<PaginationDto<UserAnswerDto>> Search(SearchUserAnswerDto dto);
    Task<UserAnswerDto?> GetById(int id);
    Task<List<UserAnswerDto>> GetAll();
}