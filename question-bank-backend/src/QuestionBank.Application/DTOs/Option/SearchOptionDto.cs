namespace QuestionBank.Application.DTOs.Option;

public class SearchOptionDto
{
    public string? Text { get; set; }
    public bool? IsCorrect { get; set; }
    public int? QuestionId { get; set; }
    public int NumberOfItemsPerPage { get; set; } = 10;
    public int CurrentPage { get; set; } = 1;
}