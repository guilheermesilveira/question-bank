namespace QuestionBank.Domain.Entities;

public class Alternative : BaseEntity
{
    public string Text { get; set; } = null!;
    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}