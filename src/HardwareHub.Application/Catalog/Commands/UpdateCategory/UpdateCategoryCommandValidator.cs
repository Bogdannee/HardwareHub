using FluentValidation;

namespace HardwareHub.Application.Catalog.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Идентификатор категории обязателен.");

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Название категории обязательно.")
                .MaximumLength(100).WithMessage("Название категории не должно превышать 100 символов.");

            RuleFor(v => v.Description)
                .MaximumLength(500).WithMessage("Описание категории не должно превышать 500 символов.");
        }
    }
}