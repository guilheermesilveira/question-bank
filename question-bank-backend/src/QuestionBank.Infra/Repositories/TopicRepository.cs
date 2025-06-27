using Microsoft.EntityFrameworkCore;
using QuestionBank.Domain.Entities;
using QuestionBank.Infra.Context;
using QuestionBank.Infra.Contracts;
using QuestionBank.Infra.Contracts.Repositories;
using QuestionBank.Infra.Pagination;

namespace QuestionBank.Infra.Repositories;

public class TopicRepository : Repository<Topic>, ITopicRepository
{
    public TopicRepository(ApplicationDbContext context) : base(context)
    {
    }

    public void Add(Topic topic)
    {
        Context.Topics.Add(topic);
    }

    public void Update(Topic topic)
    {
        Context.Topics.Update(topic);
    }

    public void Delete(Topic topic)
    {
        Context.Topics.Remove(topic);
    }

    public async Task<IPagination<Topic>> Search(string? name, int numberOfItemsPerPage = 10, int currentPage = 1)
    {
        var query = Context.Topics
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(name))
            query = query.Where(t => t.Name.Contains(name));

        var result = new Pagination<Topic>
        {
            TotalItems = await query.CountAsync(),
            NumberOfItemsPerPage = numberOfItemsPerPage,
            CurrentPage = currentPage,
            Items = await query.Skip((currentPage - 1) * numberOfItemsPerPage).Take(numberOfItemsPerPage).ToListAsync()
        };

        var numberOfPages = (double)result.TotalItems / numberOfItemsPerPage;
        result.NumberOfPages = (int)Math.Ceiling(numberOfPages);

        return result;
    }

    public async Task<Topic?> GetById(int id)
    {
        return await Context.Topics.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Topic>> GetAll()
    {
        return await Context.Topics.AsNoTracking().ToListAsync();
    }
}