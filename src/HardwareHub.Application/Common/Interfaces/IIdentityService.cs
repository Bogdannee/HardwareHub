using HardwareHub.Application.Common.Models;

namespace HardwareHub.Application.Common.Interfaces
{
    /// <summary>
    /// Интерфейс для взаимодействия с системой управления пользователями (ASP.NET Core Identity).
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Регистрирует нового пользователя в системе и возвращает результат с токеном.
        /// </summary>
        Task<AuthResult> RegisterAsync(string email, string password, string username);

        /// <summary>
        /// Выполняет вход пользователя и возвращает токен в случае успеха.
        /// </summary>
        Task<AuthResult> LoginAsync(string email, string password);
    }
}
