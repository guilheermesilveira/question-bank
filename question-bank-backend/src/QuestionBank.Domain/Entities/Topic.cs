namespace QuestionBank.Domain.Entities;

public class Topic : BaseEntity
{
    // Topic properties
    public string Name { get; set; } = null!;
    
    // Relation with Question
    public List<Question> Questions { get; set; } = null!;
}