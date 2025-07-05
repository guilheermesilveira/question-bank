using System.Reflection;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Domain.Entities;
using QuestionBank.Infra.Contracts;

namespace QuestionBank.Infra.Context;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Topic> Topics { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Alternative> Alternatives { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<bool> Commit()
    {
        return await SaveChangesAsync() > 0;
    }
}