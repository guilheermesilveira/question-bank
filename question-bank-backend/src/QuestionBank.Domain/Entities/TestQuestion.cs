namespace QuestionBank.Domain.Entities;

public class TestQuestion : BaseEntity
{
    public int TestId { get; set; }
    public Test Test { get; set; } = null!;

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public int? SelectedOptionId { get; set; }
    public Option? SelectedOption { get; set; }

    public bool? IsCorrect { get; set; }
}