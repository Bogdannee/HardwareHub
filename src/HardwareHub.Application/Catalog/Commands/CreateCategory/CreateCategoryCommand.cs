using System;
using System.Threading;
using System.Threading.Tasks;
using HardwareHub.Application.Common.Interfaces;
using HardwareHub.Domain.Entities;
using MediatR;

namespace HardwareHub.Application.Catalog.Commands.CreateCategory
{
    /// <summary>
    /// Команда для создания новой категории. Возвращает Id созданной категории.
    /// </summary>
    public record CreateCategoryCommand(string Name, string Description) : IRequest<Guid>;

    /// <summary>
    /// Обработчик команды создания категории.
    /// </summary>
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            // Создаем доменную сущность (внутри вызовется валидация бизнес-правил)
            var category = new Category(request.Name, request.Description);

            // Добавляем в контекст
            await _context.Categories.AddAsync(category, cancellationToken);
            
            // Сохраняем в БД
            await _context.SaveChangesAsync(cancellationToken);

            // Возвращаем сгенерированный Id
            return category.Id;
        }
    }
}
