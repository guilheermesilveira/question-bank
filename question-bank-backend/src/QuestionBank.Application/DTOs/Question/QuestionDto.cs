using QuestionBank.Domain.Enums;

namespace QuestionBank.Application.DTOs.Question;

public class QuestionDto
{
    public int Id { get; set; }
    public string Statement { get; set; } = null!;
    public EDifficultyLevel Difficulty { get; set; }
}