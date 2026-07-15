using FluentValidation;

namespace HardwareHub.Application.Authentication.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            // Указываем каскадный режим остановки на первой ошибке для этого правила
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop) 
                .NotEmpty().WithMessage("Email обязателен для заполнения.")
                .EmailAddress().WithMessage("Некорректный формат Email.");

            RuleFor(x => x.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Имя пользователя обязательно для заполнения.")
                .MinimumLength(3).WithMessage("Имя пользователя должно быть не менее 3 символов.")
                .MaximumLength(50).WithMessage("Имя пользователя не должно превышать 50 символов.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Пароль обязателен для заполнения.")
                .MinimumLength(6).WithMessage("Пароль должен быть не менее 6 символов.");
        }
    }
}
