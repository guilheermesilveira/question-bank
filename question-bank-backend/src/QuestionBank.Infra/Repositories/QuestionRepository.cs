using Microsoft.EntityFrameworkCore;
using QuestionBank.Domain.Entities;
using QuestionBank.Domain.Enums;
using QuestionBank.Infra.Context;
using QuestionBank.Infra.Contracts;
using QuestionBank.Infra.Contracts.Repositories;
using QuestionBank.Infra.Pagination;

namespace QuestionBank.Infra.Repositories;

public class QuestionRepository : Repository<Question>, IQuestionRepository
{
    public QuestionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public void Add(Question question)
    {
        Context.Questions.Add(question);
    }

    public void Update(Question question)
    {
        Context.Questions.Update(question);
    }

    public void Delete(Question question)
    {
        Context.Questions.Remove(question);
    }

    public async Task<IPagination<Question>> Search(
        string? statement,
        EDifficultyLevel? difficulty,
        int? topicId,
        int numberOfItemsPerPage = 10,
        int currentPage = 1)
    {
        var query = Context.Questions
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(statement))
            query = query.Where(q => q.Statement.Contains(statement));

        if (difficulty.HasValue)
            query = query.Where(q => q.Difficulty.Equals(difficulty));

        if (topicId.HasValue)
            query = query.Where(q => q.TopicId == topicId);

        var result = new Pagination<Question>
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

    public async Task<Question?> GetById(int id)
    {
        return await Context.Questions.AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<List<Question>> GetAll()
    {
        return await Context.Questions.AsNoTracking().ToListAsync();
    }
}