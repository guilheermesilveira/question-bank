using FluentValidation;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Domain.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Name)
            .NotNull()
            .WithMessage("Name cannot be null")
            .Length(3, 50)
            .WithMessage("Name must contain between {MinLength} and {MaxLength} characters");

        RuleFor(u => u.Email)
            .NotNull()
            .WithMessage("Email cannot be null")
            .EmailAddress()
            .WithMessage("Email provided is not valid");

        RuleFor(u => u.Password)
            .NotNull()
            .WithMessage("Password cannot be null")
            .Matches("^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$")
            .WithMessage("Password must be at least 8 characters long, contain one capital letter and one special character");
    }
}