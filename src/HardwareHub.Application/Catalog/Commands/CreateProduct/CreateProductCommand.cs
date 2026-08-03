using HardwareHub.Application.Common.Interfaces;
using HardwareHub.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HardwareHub.Application.Catalog.Commands.CreateProduct
{
    /// <summary>
    /// Команда создания товара. Возвращает Guid нового товара.
    /// </summary>
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        int Quantity,
        Guid CategoryId) : IRequest<Guid>;

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Проверяем, существует ли указанная категория в базе.
            // Это валидация бизнес-логики (инварианта уровня приложения)
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                throw new ArgumentException("Указанная категория не существует.");
            }

            // Создаем сущность через её конструктор (валидируем доменные правила)
            var product = new Product(
                request.Name,
                request.Description,
                request.Price,
                request.Quantity,
                request.CategoryId);

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
