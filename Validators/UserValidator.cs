using FluentValidation;
using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.Validators
{
    public class UserValidator : AbstractValidator<RegisterUserDTO>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O e-mail informado não é válido.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.")
                .Matches(@"[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
                .Matches(@"[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula.")
                .Matches(@"[0-9]").WithMessage("A senha deve conter pelo menos um número.")
                .Matches(@"[\W]").WithMessage("A senha deve conter pelo menos um caractere especial.");
        }
    }
}
