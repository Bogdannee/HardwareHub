using System;
using System.Threading;
using System.Threading.Tasks;
using HardwareHub.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HardwareHub.Application.Catalog.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : IRequest<bool>;

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            
            if (category == null)
            {
                return false;
            }

            // ПРОВЕРКА БИЗНЕС-ПРАВИЛА: Запрещено удалять категорию, если к ней привязаны товары.
            // Благодаря этому мы страхуем базу данных от ошибок внешнего ключа.
            var hasProducts = await _context.Products
                .AnyAsync(p => p.CategoryId == request.Id, cancellationToken);

            if (hasProducts)
            {
                throw new InvalidOperationException("Невозможно удалить категорию, так как к ней привязаны товары.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}