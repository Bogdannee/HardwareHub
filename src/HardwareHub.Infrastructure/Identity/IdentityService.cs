using HardwareHub.Application.Common.Interfaces;
using HardwareHub.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace HardwareHub.Infrastructure.Identity
{
    /// <summary>
    /// Реализация интерфейса IIdentityService для работы с пользователями и ролями.
    /// Служит адаптером между слоем Application и ASP.NET Core Identity.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResult> RegisterAsync(string email, string password, string username)
        {
            // 1. Создаем экземпляр нашего ApplicationUser
            var user = new ApplicationUser
            {
                Email = email,
                UserName = username
            };

            // 2. Вызываем UserManager для создания пользователя и хэширования пароля
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                // Если не удалось, собираем все ошибки, которые вернула Identity
                var errors = result.Errors.Select(e => e.Description);
                return AuthResult.Failure(errors);
            }

            // 3. По умолчанию регистрируем каждого пользователя с ролью "Customer"
            // Важно: Сама роль "Customer" должна существовать в БД! О ее создании позаботимся на шаге сидинга.
            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => e.Description);
                return AuthResult.Failure(errors);
            }

            // 4. Генерируем JWT токен для мгновенного входа после регистрации
            var roles = new[] { "Customer" };
            var token = _tokenService.GenerateJwtToken(user.Id, user.Email!, roles);

            return AuthResult.Success(token);
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            // 1. Ищем пользователя по Email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Защита от перебора: возвращаем обобщенную ошибку, не выдавая существование email
                return AuthResult.Failure(new[] { "Неверный логин или пароль." });
            }

            // 2. Проверяем правильность пароля
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
            {
                return AuthResult.Failure(new[] { "Неверный логин или пароль." });
            }

            // 3. Получаем список ролей пользователя из базы данных
            var roles = await _userManager.GetRolesAsync(user);

            // 4. Генерируем токен
            var token = _tokenService.GenerateJwtToken(user.Id, user.Email!, roles);

            return AuthResult.Success(token);
        }
    }
}
