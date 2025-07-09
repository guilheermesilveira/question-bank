namespace QuestionBank.Application.DTOs.Option;

public class AddOptionDto
{
    public string Text { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
}