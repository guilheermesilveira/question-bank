using Microsoft.EntityFrameworkCore;
using QuestionBank.Domain.Entities;
using QuestionBank.Infra.Context;
using QuestionBank.Infra.Contracts;
using QuestionBank.Infra.Contracts.Repositories;
using QuestionBank.Infra.Pagination;

namespace QuestionBank.Infra.Repositories;

public class AlternativeRepository : Repository<Alternative>, IAlternativeRepository
{
    public AlternativeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public void Add(Alternative alternative)
    {
        Context.Alternatives.Add(alternative);
    }

    public void Update(Alternative alternative)
    {
        Context.Alternatives.Update(alternative);
    }

    public async Task<IPagination<Alternative>> Search(
        string? text,
        bool? isCorrect,
        int? questionId,
        int numberOfItemsPerPage = 10,
        int currentPage = 1)
    {
        var query = Context.Alternatives
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(text))
            query = query.Where(a => a.Text.Contains(text));

        if (isCorrect.HasValue)
            query = query.Where(a => a.IsCorrect == isCorrect);
        
        if (questionId.HasValue)
            query = query.Where(a => a.QuestionId == questionId);

        var result = new Pagination<Alternative>
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

    public async Task<Alternative?> GetById(int id)
    {
        return await Context.Alternatives.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Alternative>> GetAll()
    {
        return await Context.Alternatives.AsNoTracking().ToListAsync();
    }
}