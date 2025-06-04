using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Contracts.Repositories;

public interface ITopicRepository : IRepository<Topic>
{
    void Add(Topic topic);
    void Update(Topic topic);
    Task<IPagination<Topic>> Search(string? name, int numberOfItemsPerPage = 10, int currentPage = 1);
    Task<Topic?> GetById(int id);
    Task<List<Topic>> GetAll();
}