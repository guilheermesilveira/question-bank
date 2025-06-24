namespace QuestionBank.Application.DTOs.Alternative;

public class UpdateAlternativeDto
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
}