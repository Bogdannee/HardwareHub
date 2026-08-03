using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HardwareHub.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HardwareHub.Application.Catalog.Queries.GetCategories
{
    /// <summary>
    /// Запрос на получение всех категорий.
    /// </summary>
    public record GetCategoriesQuery : IRequest<List<CategoryDto>>;

    /// <summary>
    /// Обработчик запроса получения категорий.
    /// </summary>
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetCategoriesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            // Используем .AsNoTracking() для ускорения чтения, так как данные не будут изменяться.
            // Маппим сущности в DTO на уровне базы данных с помощью .Select()
            return await _context.Categories
                .AsNoTracking()
                .Select(c => new CategoryDto(c.Id, c.Name, c.Description))
                .ToListAsync(cancellationToken);
        }
    }
}
