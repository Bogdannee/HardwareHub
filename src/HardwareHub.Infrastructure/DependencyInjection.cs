using HardwareHub.Infrastructure.Identity;
using HardwareHub.Infrastructure.Persistence;
using HardwareHub.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HardwareHub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            // 1. Извлекаем строку подключения из appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Строка подключения 'DefaultConnection' не найдена.");

            // 2. Регистрируем ApplicationDbContext с использованием провайдера PostgreSQL
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, b => 
                    // Указываем, что миграции будут создаваться и храниться в проекте Infrastructure
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            
            // 1. Настройка паттерна Options для JwtSettings
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            // 2. Регистрация наших сервисов
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IIdentityService, IdentityService>();

            // 3. Регистрируем и настраиваем ASP.NET Core Identity
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    // Настройки безопасности паролей (для удобства разработки делаем их чуть проще)
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6; // Минимальная длина пароля
                    
                    // Настройки уникальности пользователей
                    options.User.RequireUniqueEmail = true;
                })
                // Указываем Identity, где хранить данные пользователей (в нашем DbContext)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                // Добавляем провайдеры токенов по умолчанию (пригодится для генерации токенов сброса пароля или подтверждения Email)
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
