using Microsoft.EntityFrameworkCore;
using QuestionBank.Domain.Entities;
using QuestionBank.Infra.Context;
using QuestionBank.Infra.Contracts;
using QuestionBank.Infra.Contracts.Repositories;
using QuestionBank.Infra.Pagination;

namespace QuestionBank.Infra.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public void Add(User user)
    {
        Context.Users.Add(user);
    }

    public void Update(User user)
    {
        Context.Users.Update(user);
    }

    public void Delete(User user)
    {
        Context.Users.Remove(user);
    }

    public async Task<IPagination<User>> Search(
        string? name,
        string? email,
        int numberOfItemsPerPage = 10,
        int currentPage = 1)
    {
        var query = Context.Users
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(name))
            query = query.Where(u => u.Name.Contains(name));

        if (!string.IsNullOrEmpty(email))
            query = query.Where(u => u.Email.Contains(email));

        var result = new Pagination<User>
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

    public async Task<User?> GetById(int id)
    {
        return await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetAll()
    {
        return await Context.Users.AsNoTracking().ToListAsync();
    }
}