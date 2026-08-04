using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using HardwareHub.Application.Catalog.Queries.GetCategories;
using HardwareHub.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HardwareHub.Application.Catalog.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(Guid Id, string Name, string Description) : IRequest<bool>;

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                return false;
            }

            category.UpdateName(request.Name);
            category.UpdateDescription(request.Description);

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}