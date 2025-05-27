using QuestionBank.Application.DTOs.Pagination;
using QuestionBank.Application.DTOs.Question;

namespace QuestionBank.Application.Contracts.Services;

public interface IQuestionService
{
    Task<QuestionDto?> Add(AddQuestionDto dto);
    Task<QuestionDto?> Update(int id, UpdateQuestionDto dto);
    Task<PaginationDto<QuestionDto>> Search(SearchQuestionDto dto);
    Task<QuestionDto?> GetById(int id);
    Task<List<QuestionDto>> GetAll();
}