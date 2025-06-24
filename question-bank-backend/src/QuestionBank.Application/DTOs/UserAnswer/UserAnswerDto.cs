namespace QuestionBank.Application.DTOs.UserAnswer;

public class UserAnswerDto
{
    public int Id { get; set; }
    public bool IsCorrect { get; set; }
    public int UserId { get; set; }
    public int QuestionId { get; set; }
    public int AlternativeId { get; set; }
}