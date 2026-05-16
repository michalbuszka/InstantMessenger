using FluentValidation;

namespace InstantMessenger.Application.Validators;

public class LoginValidator : AbstractValidator<DTOs.LoginRegister.LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Nazwa użytkownika jest wymagana")
            .MinimumLength(3).WithMessage("Nazwa użytkownika musi mieć min. 3 znaki")
            .MaximumLength(20).WithMessage("Nazwa użytkownika jest za długa");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Hasło nie może być puste")
            .MinimumLength(8).WithMessage("Hasło musi mieć co najmniej 8 znaków")
            .Matches(@"[A-Z]").WithMessage("Hasło musi zawierać wielką literę")
            .Matches(@"[0-9]").WithMessage("Hasło musi zawierać cyfrę");
    }
}