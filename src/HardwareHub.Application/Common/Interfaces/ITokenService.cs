namespace HardwareHub.Application.Common.Interfaces
{
    /// <summary>
    /// Интерфейс для сервиса генерации токенов безопасности.
    /// Является "портом" (абстракцией) в слое Application.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Генерирует JWT-токен на основе идентификатора пользователя, его почты и ролей.
        /// </summary>
        string GenerateJwtToken(Guid userId, string email, IEnumerable<string> roles);
    }
}
