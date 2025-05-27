namespace QuestionBank.Domain.Entities;

public class UserAnswer : BaseEntity
{
    // UserAnswer properties
    public bool IsCorrect { get; set; }

    // Relation with User
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Relation with Question
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    // Relation with Alternative
    public int AlternativeId { get; set; }
    public Alternative Alternative { get; set; } = null!;
}