namespace QuestionBank.Application.DTOs.User;

public class SearchUserDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int NumberOfItemsPerPage { get; set; } = 10;
    public int CurrentPage { get; set; } = 1;
}