using QuestionBank.Application.DTOs.Alternative;
using QuestionBank.Application.DTOs.Pagination;

namespace QuestionBank.Application.Contracts.Services;

public interface IAlternativeService
{
    Task<AlternativeDto?> Add(AddAlternativeDto dto);
    Task<AlternativeDto?> Update(int id, UpdateAlternativeDto dto);
    Task<PaginationDto<AlternativeDto>> Search(SearchAlternativeDto dto);
    Task<AlternativeDto?> GetById(int id);
    Task<List<AlternativeDto>> GetAll();
}