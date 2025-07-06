using QuestionBank.Domain.Enums;

namespace QuestionBank.Application.DTOs.Test;

public class CreateTestDto
{
    public int UserId { get; set; }
    public string Title { get; set; } = null!;
    public int TotalQuestions { get; set; }
    public EDifficultyLevel Difficulty { get; set; }
    public List<int> TopicIds { get; set; } = null!;
}