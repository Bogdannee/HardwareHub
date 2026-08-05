using System;
using FluentAssertions;
using HardwareHub.Domain.Entities;
using Xunit;

namespace HardwareHub.Application.UnitTests.Catalog.Entities
{
    /// <summary>
    /// Тесты для проверки бизнес-правил (инвариантов) сущности Product.
    /// </summary>
    public class ProductTests
    {
        [Fact]
        public void Constructor_ShouldCreateProduct_WhenParametersAreValid()
        {
            // Arrange
            var name = "Intel Core i5-13600K";
            var description = "Отличный процессор для игр";
            var price = 320.00m;
            var quantity = 10;
            var categoryId = Guid.NewGuid();

            // Act
            var product = new Product(name, description, price, quantity, categoryId);

            // Assert
            product.Should().NotBeNull();
            product.Id.Should().NotBeEmpty();
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
            product.Price.Should().Be(price);
            product.Quantity.Should().Be(quantity);
            product.CategoryId.Should().Be(categoryId);
        }

        [Theory]
        [InlineData(-10.50)]
        [InlineData(-0.01)]
        public void UpdatePrice_ShouldThrowArgumentException_WhenPriceIsNegative(decimal invalidPrice)
        {
            // Arrange
            var product = new Product("Test", "Desc", 100m, 5, Guid.NewGuid());

            // Act
            Action action = () => product.UpdatePrice(invalidPrice);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("Цена не может быть отрицательной.");
        }

        [Fact]
        public void UpdateQuantity_ShouldThrowArgumentException_WhenQuantityIsNegative()
        {
            // Arrange
            var product = new Product("Test", "Desc", 100m, 5, Guid.NewGuid());

            // Act
            Action action = () => product.UpdateQuantity(-1);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("Количество не может быть отрицательным.");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCategoryIdIsEmpty()
        {
            // Arrange, Act
            Action action = () => new Product("Test", "Desc", 100m, 5, Guid.Empty);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("*категории*");
        }
    }
}
