using FluentValidation;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Domain.Validators;

public class TopicValidator : AbstractValidator<Topic>
{
    public TopicValidator()
    {
        RuleFor(t => t.Name)
            .NotNull()
            .WithMessage("Name cannot be null")
            .MaximumLength(100)
            .WithMessage("Name cannot be longer than 100 characters");
    }
}