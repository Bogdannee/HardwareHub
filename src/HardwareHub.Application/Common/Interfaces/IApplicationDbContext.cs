using System.Threading;
using System.Threading.Tasks;
using HardwareHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HardwareHub.Application.Common.Interfaces
{
    /// <summary>
    /// Интерфейс контекста базы данных для слоя Application.
    /// Позволяет слою бизнес-логики работать с таблицами без прямой зависимости от EF Core Infrastructure.
    /// </summary>
    public interface IApplicationDbContext
    {
        DbSet<Category> Categories { get; }
        DbSet<Product> Products { get; }

        /// <summary>
        /// Сохраняет все изменения, сделанные в этом контексте, в базу данных.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
