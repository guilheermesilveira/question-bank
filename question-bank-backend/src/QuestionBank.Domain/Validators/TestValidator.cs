using FluentValidation;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Domain.Validators;

public class TestValidator : AbstractValidator<Test>
{
    public TestValidator()
    {
        RuleFor(t => t.Title)
            .NotNull()
            .WithMessage("Title cannot be null")
            .MaximumLength(50)
            .WithMessage("Title cannot be longer than 50 characters");

        RuleFor(t => t.TotalQuestions)
            .NotNull()
            .WithMessage("TotalQuestions cannot be null")
            .GreaterThanOrEqualTo(1)
            .WithMessage("TotalQuestions must be greater than or equal to 1")
            .LessThanOrEqualTo(20)
            .WithMessage("TotalQuestions must be less than or equal to 20");
    }
}