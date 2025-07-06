using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Contracts.Repositories;

public interface ITestRepository : IRepository<Test>
{
    void Add(Test test);
    void Update(Test test);
    Task<Test?> GetById(int id);
}