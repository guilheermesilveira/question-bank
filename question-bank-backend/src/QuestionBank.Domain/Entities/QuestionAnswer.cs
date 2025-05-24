namespace QuestionBank.Domain.Entities;

public class QuestionAnswer : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public int? SelectedAlternativeId { get; set; }
    public Alternative? SelectedAlternative { get; set; }
    public bool IsCorrect { get; set; }
}