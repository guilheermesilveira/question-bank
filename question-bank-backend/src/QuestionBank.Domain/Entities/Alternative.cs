namespace QuestionBank.Domain.Entities;

public class Alternative : BaseEntity
{
    // Alternative properties
    public string Text { get; set; } = null!;
    public bool IsCorrect { get; set; }

    // Relation with Question
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    // Relation with UserAnswer
    public List<UserAnswer> UserAnswers { get; set; } = null!;
}