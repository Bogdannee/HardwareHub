using HardwareHub.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HardwareHub.Application.Catalog.Commands.UpdateProduct
{
    /// <summary>
    /// Команда для обновления данных товара. Возвращает bool (успешно ли обновление).
    /// </summary>
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Quantity,
        Guid CategoryId) : IRequest<bool>;

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (product == null)
            {
                return false;
            }

            if (product.CategoryId != request.CategoryId)
            {
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

                if (!categoryExists)
                {
                    throw new ArgumentException("Указанная категория не существует.");
                }

                product.UpdateCategory(request.CategoryId);
            }

            product.UpdateName(request.Name);
            product.UpdateDescription(request.Description);
            product.UpdatePrice(request.Price);
            product.UpdateQuantity(request.Quantity);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
