using System;
using System.Threading;
using System.Threading.Tasks;
using HardwareHub.Application.Catalog.Queries.GetProducts;
using HardwareHub.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HardwareHub.Application.Catalog.Queries.GetProductById
{
    /// <summary>
    /// Запрос деталей конкретного товара. Если товар не найден — вернет null.
    /// </summary>
    public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IApplicationDbContext _context;

        public GetProductByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.Id == request.Id)
                .Select(p => new ProductDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.Quantity,
                    p.CategoryId,
                    p.Category.Name))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
