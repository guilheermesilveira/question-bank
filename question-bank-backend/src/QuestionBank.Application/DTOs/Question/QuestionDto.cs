namespace QuestionBank.Application.DTOs.Question;

public class QuestionDto
{
    public int Id { get; set; }
    public string Statement { get; set; } = null!;
    public string Difficulty { get; set; } = null!;
    public int TopicId { get; set; }
}