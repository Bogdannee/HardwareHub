using FluentAssertions;
using HardwareHub.Application.Catalog.Commands.CreateProduct;
using HardwareHub.Application.Common.Interfaces;
using HardwareHub.Domain.Entities;
using MockQueryable.NSubstitute; // Полезная библиотека для мока DbSet (если установлена)
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HardwareHub.Application.UnitTests.Catalog.Commands.CreateProduct
{
    public class CreateProductCommandHandlerTests
    {
        private readonly IApplicationDbContext _contextMock;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _contextMock = Substitute.For<IApplicationDbContext>();
            _handler = new CreateProductCommandHandler(_contextMock);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenCategoryDoesNotExist()
        {
            // Arrange: Создаем обычный пустой список категорий
            var categories = new List<Category>();
            
            // Вызываем BuildMockDbSet напрямую у списка List<Category>.
            // Компилятор сразу поймет, что TEntity — это Category.
            var mockDbSet = categories.BuildMockDbSet(); 
            
            _contextMock.Categories.Returns(mockDbSet);

            var command = new CreateProductCommand(
                "GeForce RTX 4080", 
                "Super GPU", 
                1200m, 
                5, 
                Guid.NewGuid() // Случайный ID несуществующей категории
            );

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Указанная категория не существует.");

            // Убеждаемся, что SaveChangesAsync НЕ вызывался
            await _contextMock.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
