using FluentValidation;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Domain.Validators;

public class QuestionValidator : AbstractValidator<Question>
{
    public QuestionValidator()
    {
        RuleFor(q => q.Statement)
            .NotNull()
            .WithMessage("Statement cannot be null");

        RuleFor(q => q.Difficulty)
            .IsInEnum()
            .WithMessage("Difficulty must be either 'Easy' or 'Medium' or 'Hard'")
            .NotNull()
            .WithMessage("Difficulty cannot be null");
    }
}