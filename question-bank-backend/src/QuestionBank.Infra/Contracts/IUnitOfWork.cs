namespace QuestionBank.Infra.Contracts;

public interface IUnitOfWork
{
    Task<bool> Commit();
}