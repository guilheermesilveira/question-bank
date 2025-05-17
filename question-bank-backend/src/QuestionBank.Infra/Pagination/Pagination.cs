using QuestionBank.Domain.Entities;
using QuestionBank.Infra.Contracts;

namespace QuestionBank.Infra.Pagination;

public class Pagination<T> : IPagination<T> where T : BaseEntity, new()
{
    public int TotalItems { get; set; }
    public int NumberOfItemsPerPage { get; set; }
    public int NumberOfPages { get; set; }
    public int CurrentPage { get; set; }
    public List<T> Items { get; set; } = new();
}