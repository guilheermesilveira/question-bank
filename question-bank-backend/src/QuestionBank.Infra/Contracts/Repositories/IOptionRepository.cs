using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Contracts.Repositories;

public interface IOptionRepository : IRepository<Option>
{
    void Add(Option option);
    void Update(Option option);
    void Delete(Option option);
    Task<IPagination<Option>> Search(string? text, bool? isCorrect, int? questionId, int numberOfItemsPerPage = 10,
        int currentPage = 1);
    Task<Option?> GetById(int id);
    Task<List<Option>> GetAll();
}