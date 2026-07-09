using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HardwareHub.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HardwareHub.Infrastructure.Identity
{
    /// <summary>
    /// Реализация сервиса генерации JWT токенов.
    /// Использует стандартную библиотеку Microsoft.IdentityModel.Tokens.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        // Внедряем настройки JWT через IOptions для соблюдения Clean Architecture и DI-паттернов
        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateJwtToken(Guid userId, string email, IEnumerable<string> roles)
        {
            // 1. Создаем список утверждений (Claims) о пользователе
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), // Уникальный Id субъекта
                new Claim(JwtRegisteredClaimNames.Email, email),           // Email пользователя
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Уникальный идентификатор токена (защита от replay-атак)
            };

            // Добавляем роли пользователя в Claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 2. Создаем симметричный ключ безопасности на основе нашего секретного ключа
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            
            // 3. Задаем алгоритм подписи (HMAC с SHA-256)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 4. Описываем конфигурацию нашего токена
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes), // Срок действия
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = creds
            };

            // 5. Генерируем токен и преобразуем его в строку
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
