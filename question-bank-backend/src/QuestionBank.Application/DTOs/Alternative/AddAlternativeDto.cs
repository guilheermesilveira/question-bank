namespace QuestionBank.Application.DTOs.Alternative;

public class AddAlternativeDto
{
    public string Text { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
}