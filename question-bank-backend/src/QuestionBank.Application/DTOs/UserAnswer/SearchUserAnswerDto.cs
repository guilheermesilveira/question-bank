namespace QuestionBank.Application.DTOs.UserAnswer;

public class SearchUserAnswerDto
{
    public bool? IsCorrect { get; set; }
    public int? UserId { get; set; }
    public int? QuestionId { get; set; }
    public int? AlternativeId { get; set; }
    public int NumberOfItemsPerPage { get; set; } = 10;
    public int CurrentPage { get; set; } = 1;
}