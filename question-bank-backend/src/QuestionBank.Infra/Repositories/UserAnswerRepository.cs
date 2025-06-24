using Microsoft.EntityFrameworkCore;
using QuestionBank.Domain.Entities;
using QuestionBank.Infra.Context;
using QuestionBank.Infra.Contracts;
using QuestionBank.Infra.Contracts.Repositories;
using QuestionBank.Infra.Pagination;

namespace QuestionBank.Infra.Repositories;

public class UserAnswerRepository : Repository<UserAnswer>, IUserAnswerRepository
{
    public UserAnswerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public void Add(UserAnswer userAnswer)
    {
        Context.UserAnswers.Add(userAnswer);
    }

    public async Task<IPagination<UserAnswer>> Search(
        bool? isCorrect,
        int? userId,
        int? questionId,
        int? alternativeId,
        int numberOfItemsPerPage = 10,
        int currentPage = 1)
    {
        var query = Context.UserAnswers
            .AsNoTracking()
            .AsQueryable();

        if (isCorrect.HasValue)
            query = query.Where(ua => ua.IsCorrect == isCorrect);

        if (userId.HasValue)
            query = query.Where(ua => ua.UserId == userId);

        if (questionId.HasValue)
            query = query.Where(ua => ua.QuestionId == questionId);

        if (alternativeId.HasValue)
            query = query.Where(ua => ua.AlternativeId == alternativeId);

        var result = new Pagination<UserAnswer>
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

    public async Task<UserAnswer?> GetById(int id)
    {
        return await Context.UserAnswers.AsNoTracking().FirstOrDefaultAsync(ua => ua.Id == id);
    }

    public async Task<List<UserAnswer>> GetAll()
    {
        return await Context.UserAnswers.AsNoTracking().ToListAsync();
    }
}