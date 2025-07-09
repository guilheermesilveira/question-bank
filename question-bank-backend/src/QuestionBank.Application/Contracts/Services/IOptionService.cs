using QuestionBank.Application.DTOs.Option;
using QuestionBank.Application.DTOs.Pagination;

namespace QuestionBank.Application.Contracts.Services;

public interface IOptionService
{
    Task<OptionDto?> Add(AddOptionDto dto);
    Task<OptionDto?> Update(int id, UpdateOptionDto dto);
    Task Delete(int id);
    Task<PaginationDto<OptionDto>> Search(SearchOptionDto dto);
    Task<OptionDto?> GetById(int id);
    Task<List<OptionDto>> GetAll();
}