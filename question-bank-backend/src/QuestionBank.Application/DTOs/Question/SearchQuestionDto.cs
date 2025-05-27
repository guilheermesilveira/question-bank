using QuestionBank.Domain.Enums;

namespace QuestionBank.Application.DTOs.Question;

public class SearchQuestionDto
{
    public int? Id { get; set; }
    public string? Statement { get; set; } = null!;
    public EDifficultyLevel? Difficulty { get; set; }
    public int NumberOfItemsPerPage { get; set; } = 10;
    public int CurrentPage { get; set; } = 1;
}