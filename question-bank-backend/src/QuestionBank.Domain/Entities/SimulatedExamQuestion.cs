namespace QuestionBank.Domain.Entities;

public class SimulatedExamQuestion : BaseEntity
{
    public int SimulatedExamId { get; set; }
    public SimulatedExam SimulatedExam { get; set; } = null!;

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public int? SelectedAlternativeId { get; set; }
    public Alternative? SelectedAlternative { get; set; }

    public bool? IsCorrect { get; set; }
}