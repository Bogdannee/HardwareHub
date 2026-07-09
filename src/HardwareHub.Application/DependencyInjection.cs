using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace HardwareHub.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Получаем текущую сборку (Assembly) Application
            var assembly = Assembly.GetExecutingAssembly();

            // 1. Регистрируем MediatR. Он автоматически просканирует сборку и зарегистрирует все IRequestHandler
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            // 2. Регистрируем FluentValidation. Сканирует сборку и находит все AbstractValidator
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
