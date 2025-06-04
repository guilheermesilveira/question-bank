using FluentValidation;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Domain.Validators;

public class AlternativeValidator : AbstractValidator<Alternative>
{
    public AlternativeValidator()
    {
        RuleFor(a => a.Text)
            .NotNull()
            .WithMessage("Text cannot be null");

        RuleFor(a => a.IsCorrect)
            .NotNull()
            .WithMessage("IsCorrect cannot be null");
    }
}