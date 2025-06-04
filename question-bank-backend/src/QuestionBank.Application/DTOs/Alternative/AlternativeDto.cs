namespace QuestionBank.Application.DTOs.Alternative;

public class AlternativeDto
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public bool IsCorrect { get; set; }
}