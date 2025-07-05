using QuestionBank.Domain.Enums;

namespace QuestionBank.Domain.Entities;

public class Question : BaseEntity
{
    public string Statement { get; set; } = null!;
    public EDifficultyLevel Difficulty { get; set; }

    public int TopicId { get; set; }
    public Topic Topic { get; set; } = null!;

    public List<Alternative> Alternatives { get; set; } = null!;
}