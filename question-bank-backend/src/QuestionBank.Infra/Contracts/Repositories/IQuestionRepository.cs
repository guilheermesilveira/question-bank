using QuestionBank.Domain.Entities;
using QuestionBank.Domain.Enums;

namespace QuestionBank.Infra.Contracts.Repositories;

public interface IQuestionRepository : IRepository<Question>
{
    void Add(Question question);
    void Update(Question question);
    void Delete(Question question);
    Task<IPagination<Question>> Search(string? statement, EDifficultyLevel? difficulty, int? topicId,
        int numberOfItemsPerPage = 10, int currentPage = 1);
    Task<Question?> GetById(int id);
    Task<List<Question>> GetAll();
}