namespace QuestionBank.Domain.Entities;

public class Option : BaseEntity
{
    public string Text { get; set; } = null!;
    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}