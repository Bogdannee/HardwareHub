namespace HardwareHub.Infrastructure.Identity
{
    /// <summary>
    /// Класс для сопоставления настроек JWT из appsettings.json.
    /// Использование Options Pattern делает код чистым и строго типизированным.
    /// </summary>
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";
        
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; }
    }
}
