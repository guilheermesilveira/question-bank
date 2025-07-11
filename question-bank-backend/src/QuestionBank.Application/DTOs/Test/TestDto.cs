namespace QuestionBank.Application.DTOs.Test;

public class TestDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int TotalQuestions { get; set; }
    public int NumberOfCorrectAnswers { get; set; }
    public string Status { get; set; } = null!;
    public int UserId { get; set; }
    public List<TestQuestionDto> TestQuestions { get; set; } = null!;
}