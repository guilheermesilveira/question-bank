using Microsoft.EntityFrameworkCore;
using QuestionBank.Domain.Entities;
using QuestionBank.Infra.Context;
using QuestionBank.Infra.Contracts.Repositories;

namespace QuestionBank.Infra.Repositories;

public class TestRepository : Repository<Test>, ITestRepository
{
    public TestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public void Add(Test test)
    {
        Context.Tests.Add(test);
    }

    public void Update(Test test)
    {
        Context.Tests.Update(test);
    }

    public async Task<Test?> GetById(int id)
    {
        return await Context.Tests
            .Where(t => t.Id == id)
            .Include(t => t.TestQuestions)
                .ThenInclude(tq => tq.Question)
                    .ThenInclude(q => q.Options)
            .Include(t => t.TestQuestions)
                .ThenInclude(tq => tq.SelectedOption)
            .FirstOrDefaultAsync();
    }
}