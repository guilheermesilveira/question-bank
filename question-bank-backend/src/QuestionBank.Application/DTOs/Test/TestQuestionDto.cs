using QuestionBank.Application.DTOs.Option;
using QuestionBank.Application.DTOs.Question;

namespace QuestionBank.Application.DTOs.Test;

public class TestQuestionDto
{
    public QuestionDto Question { get; set; } = null!;
    public OptionDto? SelectedOption { get; set; }
    public bool? IsCorrect { get; set; }
}