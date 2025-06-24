using QuestionBank.Domain.Enums;

namespace QuestionBank.Application.DTOs.Question;

public class SearchQuestionDto
{
    public string? Statement { get; set; } = null!;
    public EDifficultyLevel? Difficulty { get; set; }
    public int? TopicId { get; set; }
    public int NumberOfItemsPerPage { get; set; } = 10;
    public int CurrentPage { get; set; } = 1;
}