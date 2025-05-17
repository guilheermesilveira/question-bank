namespace QuestionBank.Application.DTOs.User;

public record SearchUserDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}