using FluentValidation;

namespace HardwareHub.Application.Catalog.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Идентификатор товара обязателен.");

            RuleFor(v => v.Name).NotEmpty().WithMessage("Имя товара обязательно.")
                .MaximumLength(150).WithMessage("Имя товара не должно превышать 150 символов.");

            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Описание товара обязательно.")
                .MaximumLength(1000).WithMessage("Описание товара не должно превышать 1000 символов.");

            RuleFor(v => v.Price)
                .GreaterThan(0).WithMessage("Цена должна быть больше нуля.");
            
            RuleFor(v => v.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Количество товара не может быть отрицательным.");

            RuleFor(v => v.CategoryId)
                .NotEmpty().WithMessage("Идентификатор категории обязателен.");
        }
    }
}
