using QuestionBank.Domain.Enums;

namespace QuestionBank.Domain.Entities;

public class SimulatedExam : BaseEntity
{
    public string Title { get; set; } = null!;
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public ESimulatedExamStatus Status { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public List<SimulatedExamQuestion> SimulatedExamQuestions { get; set; } = null!;
}