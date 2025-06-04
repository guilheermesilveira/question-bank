using FluentValidation;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Domain.Validators;

public class TopicValidator : AbstractValidator<Topic>
{
    public TopicValidator()
    {
        RuleFor(t => t.Name)
            .NotNull()
            .WithMessage("Name cannot be null");
    }
}