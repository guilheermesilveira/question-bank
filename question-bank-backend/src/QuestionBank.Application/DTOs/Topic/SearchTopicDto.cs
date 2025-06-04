namespace QuestionBank.Application.DTOs.Topic;

public class SearchTopicDto
{
    public string? Name { get; set; }
    public int NumberOfItemsPerPage { get; set; } = 10;
    public int CurrentPage { get; set; } = 1;
}