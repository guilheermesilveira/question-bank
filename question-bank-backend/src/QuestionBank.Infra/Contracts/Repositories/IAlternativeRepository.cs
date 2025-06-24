using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Contracts.Repositories;

public interface IAlternativeRepository : IRepository<Alternative>
{
    void Add(Alternative alternative);
    void Update(Alternative alternative);
    Task<IPagination<Alternative>> Search(string? text, bool? isCorrect, int? questionId, int numberOfItemsPerPage = 10,
        int currentPage = 1);
    Task<Alternative?> GetById(int id);
    Task<List<Alternative>> GetAll();
}