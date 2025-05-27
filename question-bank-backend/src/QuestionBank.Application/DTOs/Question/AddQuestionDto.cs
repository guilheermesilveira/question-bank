using QuestionBank.Domain.Enums;

namespace QuestionBank.Application.DTOs.Question;

public class AddQuestionDto
{
    public string Statement { get; set; } = null!;
    public EDifficultyLevel Difficulty { get; set; }
}