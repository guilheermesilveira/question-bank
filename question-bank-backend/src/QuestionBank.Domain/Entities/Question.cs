using QuestionBank.Domain.Enums;

namespace QuestionBank.Domain.Entities;

public class Question : BaseEntity
{
    // Question properties
    public string Statement { get; set; } = null!;
    public EDifficultyLevel Difficulty { get; set; }

    // Relation with Topic
    public int TopicId { get; set; }
    public Topic Topic { get; set; } = null!;

    // Relation with Alternative
    public List<Alternative> Alternatives { get; set; } = null!;
    
    // Relation with UserAnswer
    public List<UserAnswer> UserAnswers { get; set; } = null!;
}