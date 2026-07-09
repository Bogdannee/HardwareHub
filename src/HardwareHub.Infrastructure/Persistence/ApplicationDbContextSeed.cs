using HardwareHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace HardwareHub.Infrastructure.Persistence
{
    /// <summary>
    /// Класс для первичного наполнения (сидинга) базы данных.
    /// Гарантирует наличие базовых ролей и дефолтных пользователей при старте приложения.
    /// </summary>
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedIdentityAsync(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            // 1. Инициализируем роли
            var roles = new[] { "Admin", "Customer" };

            foreach (var roleName in roles)
            {
                // Проверяем, существует ли роль в базе данных
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    // Если нет — создаем ее
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }

            // 2. Инициализируем дефолтного администратора (для тестов)
            var adminEmail = "admin@hardwarehub.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // Создаем админа с дефолтным паролем (в проде пароли берут из переменных окружения!)
                var result = await userManager.CreateAsync(adminUser, "AdminPass123!");
                
                if (result.Succeeded)
                {
                    // Назначаем роль Admin
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
