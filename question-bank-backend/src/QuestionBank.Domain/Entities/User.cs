namespace QuestionBank.Domain.Entities;

public class User : BaseEntity
{
    // User properties
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsAdmin { get; set; }

    // Relation with UserAnswer
    public List<UserAnswer> UserAnswers { get; set; } = null!;
}