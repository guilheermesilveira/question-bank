using QuestionBank.Domain.Enums;

namespace QuestionBank.Domain.Entities;

public class Test : BaseEntity
{
    public string Title { get; set; } = null!;
    public int TotalQuestions { get; set; }
    public int NumberOfCorrectAnswers { get; set; }
    public ETestStatus Status { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public List<TestQuestion> TestQuestions { get; set; } = null!;
}