using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Contracts;

public interface IPagination<T> where T : BaseEntity, new()
{
    int TotalItems { get; set; }
    int NumberOfItemsPerPage { get; set; }
    int NumberOfPages { get; set; }
    int CurrentPage { get; set; }
    List<T> Items { get; set; }
}