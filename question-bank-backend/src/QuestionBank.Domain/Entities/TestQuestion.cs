namespace QuestionBank.Domain.Entities;

public class TestQuestion : BaseEntity
{
    public int TestId { get; set; }
    public Test Test { get; set; } = null!;

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public int? SelectedAlternativeId { get; set; }
    public Alternative? SelectedAlternative { get; set; }

    public bool? IsCorrect { get; set; }
}