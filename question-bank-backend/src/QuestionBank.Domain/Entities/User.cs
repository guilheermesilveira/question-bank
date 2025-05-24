namespace QuestionBank.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsAdmin { get; set; }
    public List<QuestionAnswer> Answers { get; set; } = null!;
}