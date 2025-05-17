using FluentValidation;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Domain.Validators;

public class LoginValidator : AbstractValidator<User>
{
    public LoginValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
    }
}