using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Contracts.Repositories;

public interface IUserRepository : IRepository<User>
{
    void Add(User user);
    void Update(User user);
    Task<IPagination<User>> Search(int? id, string? name, string? email, int numberOfItemsPerPage = 10,
        int currentPage = 1);
    Task<List<User>> GetAll();
}