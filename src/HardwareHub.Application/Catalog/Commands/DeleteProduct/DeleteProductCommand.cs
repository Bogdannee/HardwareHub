using HardwareHub.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HardwareHub.Application.Catalog.Commands.DeleteProduct
{
    /// <summary>
    /// Команда на удаление товара по Id. Возвращает bool.
    /// </summary>
    public record DeleteProductCommand(Guid id) : IRequest<bool>;

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.id, cancellationToken);

            if (product == null)
            {
                return false; // Товар не найден
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}