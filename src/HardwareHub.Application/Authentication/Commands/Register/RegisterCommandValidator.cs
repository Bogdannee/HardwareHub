using FluentValidation;

namespace HardwareHub.Application.Authentication.Commands.Register
{
    /// <summary>
    /// Валидатор для команды регистрации.
    /// Выполняется автоматически на слое Application перед обработкой команды.
    /// </summary>
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен для заполнения.")
                .EmailAddress().WithMessage("Некорректный формат Email.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Имя пользователя обязательно для заполнения.")
                .MinimumLength(3).WithMessage("Имя пользователя должно быть не менее 3 символов.")
                .MaximumLength(50).WithMessage("Имя пользователя не должно превышать 50 символов.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен для заполнения.")
                .MinimumLength(6).WithMessage("Пароль должен быть не менее 6 символов.");
        }
    }
}
