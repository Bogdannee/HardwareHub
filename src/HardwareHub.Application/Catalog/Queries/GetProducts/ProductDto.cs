using System;

namespace HardwareHub.Application.Catalog.Queries.GetProducts
{
    /// <summary>
    /// DTO товара для вывода на фронтенд/клиент.
    /// Обрати внимание, что мы сразу вытягиваем и Name категории, чтобы клиенту было удобно выводить её на экран.
    /// </summary>
    public record ProductDto(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Quantity,
        Guid CategoryId,
        string CategoryName);
}