using HardwareHub.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HardwareHub.Application.Catalog.Queries.GetProducts
{
    /// <summary>
    /// Запрос на получение списка товаров. 
    /// Поддерживает фильтрацию по категории и поиск по строке в названии или описании.
    /// </summary>
    public record GetProductsQuery(
        Guid? CategoryId = null, 
        string? SearchTerm = null) : IRequest<List<ProductDto>>;

    /// <summary>
    /// Обработчик запроса получения товаров.
    /// </summary>
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            // Начинаем строить IQueryable запрос к таблице товаров.
            // .Include подгружает связанную категорию (Eager Loading), чтобы избежать N+1 проблемы с запросами к БД.
            var query = _context.Products
                .Include(p => p.Category)
                .AsNoTracking();

            // 1. Фильтрация по категории (если передана)
            if (request.CategoryId.HasValue && request.CategoryId != Guid.Empty)
            {
                query = query.Where(p => p.CategoryId == request.CategoryId.Value);
            }

            // 2. Поиск по подстроке (без учета регистра в PostgreSQL / EF Core)
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term) || 
                                         p.Description.ToLower().Contains(term));
            }

            // 3. Проецируем результат в ProductDto и выполняем запрос в БД
            return await query
                .Select(p => new ProductDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.Quantity,
                    p.CategoryId,
                    p.Category.Name))
                .ToListAsync(cancellationToken);
        }
    }
}
