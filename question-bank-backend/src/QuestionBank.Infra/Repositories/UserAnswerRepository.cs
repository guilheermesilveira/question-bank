using Microsoft.EntityFrameworkCore;
using QuestionBank.Domain.Entities;
using QuestionBank.Infra.Context;
using QuestionBank.Infra.Contracts.Repositories;

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

    public async Task<UserAnswer?> GetById(int id)
    {
        return await Context.UserAnswers.AsNoTracking().FirstOrDefaultAsync(ua => ua.Id == id);
    }

    public async Task<List<UserAnswer>> GetAll()
    {
        return await Context.UserAnswers.AsNoTracking().ToListAsync();
    }
}