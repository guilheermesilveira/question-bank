using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Contracts.Repositories;

public interface IUserAnswerRepository : IRepository<UserAnswer>
{
    void Add(UserAnswer userAnswer);
    Task<IPagination<UserAnswer>> Search(bool? isCorrect, int? userId, int? questionId, int? alternativeId,
        int numberOfItemsPerPage = 10, int currentPage = 1);
    Task<UserAnswer?> GetById(int id);
    Task<List<UserAnswer>> GetAll();
}