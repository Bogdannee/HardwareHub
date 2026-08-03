using FluentValidation;

namespace HardwareHub.Application.Catalog.Commands.CreateCategory
{
    /// <summary>
    /// Валидатор входных данных для команды создания категории.
    /// </summary>
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Название категории обязательно для заполнения.")
                .MaximumLength(100).WithMessage("Название категории не должно превышать 100 символов.");

            RuleFor(v => v.Description)
                .MaximumLength(500).WithMessage("Описание категории не должно превышать 500 символов.");
        }
    }
}
