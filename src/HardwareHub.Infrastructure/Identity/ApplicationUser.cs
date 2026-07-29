using Microsoft.AspNetCore.Identity;

namespace HardwareHub.Infrastructure.Identity
{
    /// <summary>
    /// Пользовательская сущность пользователя для ASP.NET Core Identity.
    /// Наследуется от IdentityUser с явным указанием Guid в качестве первичного ключа.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        // Здесь в будущем можно добавить кастомные свойства, например:
        // public string FirstName { get; set; } = string.Empty;
        // public string LastName { get; set; } = string.Empty;
    }
}
