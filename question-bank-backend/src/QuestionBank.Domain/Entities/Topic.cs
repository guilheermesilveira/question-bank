namespace QuestionBank.Domain.Entities;

public class Topic : BaseEntity
{
    public string Name { get; set; } = null!;
    public List<Question> Questions { get; set; } = null!;
}