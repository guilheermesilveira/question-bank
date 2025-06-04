namespace QuestionBank.Application.DTOs.Alternative;

public class SearchAlternativeDto
{
    public string? Text { get; set; }
    public bool? IsCorrect { get; set; }
    public int NumberOfItemsPerPage { get; set; } = 10;
    public int CurrentPage { get; set; } = 1;
}