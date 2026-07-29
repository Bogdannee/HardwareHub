using HardwareHub.Domain.Entities;
using HardwareHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HardwareHub.Infrastructure.Persistence
{
    /// <summary>
    /// Контекст базы данных для всего приложения.
    /// Наследуется от IdentityDbContext для автоматической поддержки таблиц аутентификации и авторизации ASP.NET Core Identity.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<Product> Products { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // КРАЙНЕ ВАЖНО: Вызов base.OnModelCreating(builder) обязателен при наследовании от IdentityDbContext!
            // Он настраивает стандартные схемы и связи для таблиц Identity (Users, Roles, Claims и т.д.).
            base.OnModelCreating(builder);

            // Применяем все конфигурации сущностей (Fluent API) из текущей сборки (Infrastructure)
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
