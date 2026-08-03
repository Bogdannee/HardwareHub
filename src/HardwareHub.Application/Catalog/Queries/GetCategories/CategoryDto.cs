using System;

namespace HardwareHub.Application.Catalog.Queries.GetCategories
{
    /// <summary>
    /// DTO категории для передачи данных на сторону клиента.
    /// Позволяет не возвращать доменную сущность напрямую.
    /// </summary>
    public record CategoryDto(
        Guid Id,
        string Name,
        string Description);
}