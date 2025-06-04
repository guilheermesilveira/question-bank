using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Contracts.Repositories;

public interface IUserAnswerRepository : IRepository<UserAnswer>
{
    void Add(UserAnswer userAnswer);
    Task<UserAnswer?> GetById(int id);
    Task<List<UserAnswer>> GetAll();
}