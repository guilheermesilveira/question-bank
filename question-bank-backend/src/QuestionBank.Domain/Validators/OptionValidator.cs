using FluentValidation;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Domain.Validators;

public class OptionValidator : AbstractValidator<Option>
{
    public OptionValidator()
    {
        RuleFor(o => o.Text)
            .NotNull()
            .WithMessage("Text cannot be null");

        RuleFor(o => o.IsCorrect)
            .NotNull()
            .WithMessage("IsCorrect cannot be null");
    }
}