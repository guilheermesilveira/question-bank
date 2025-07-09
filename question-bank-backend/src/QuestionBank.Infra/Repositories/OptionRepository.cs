using Microsoft.EntityFrameworkCore;
using QuestionBank.Domain.Entities;
using QuestionBank.Infra.Context;
using QuestionBank.Infra.Contracts;
using QuestionBank.Infra.Contracts.Repositories;
using QuestionBank.Infra.Pagination;

namespace QuestionBank.Infra.Repositories;

public class OptionRepository : Repository<Option>, IOptionRepository
{
    public OptionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public void Add(Option option)
    {
        Context.Options.Add(option);
    }

    public void Update(Option option)
    {
        Context.Options.Update(option);
    }

    public void Delete(Option option)
    {
        Context.Options.Remove(option);
    }

    public async Task<IPagination<Option>> Search(
        string? text,
        bool? isCorrect,
        int? questionId,
        int numberOfItemsPerPage = 10,
        int currentPage = 1)
    {
        var query = Context.Options
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(text))
            query = query.Where(o => o.Text.Contains(text));

        if (isCorrect.HasValue)
            query = query.Where(o => o.IsCorrect == isCorrect);

        if (questionId.HasValue)
            query = query.Where(o => o.QuestionId == questionId);

        var result = new Pagination<Option>
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

    public async Task<Option?> GetById(int id)
    {
        return await Context.Options.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Option>> GetAll()
    {
        return await Context.Options.AsNoTracking().ToListAsync();
    }
}