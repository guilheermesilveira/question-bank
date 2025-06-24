using FluentValidation;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Domain.Validators;

public class UserAnswerValidator : AbstractValidator<UserAnswer>
{
    public UserAnswerValidator()
    {
        RuleFor(ua => ua.IsCorrect)
            .NotNull()
            .WithMessage("IsCorrect cannot be null");
    }
}