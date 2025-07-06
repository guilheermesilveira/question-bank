namespace QuestionBank.Application.DTOs.Test;

public class FinishTestDto
{
    public int TestId { get; set; }
    public List<AnswerDto> Answers { get; set; } = null!;
}