using HardwareHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HardwareHub.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Конфигурация Fluent API для сущности Product.
    /// Позволяет разделить доменную модель от деталей хранения в БД (согласно Clean Architecture).
    /// </summary>
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Задаем имя таблицы в БД
            builder.ToTable("Products");

            // Настройка первичного ключа
            builder.HasKey(p => p.Id);

            // Настройка свойств сущности Product
            builder.Property(p => p.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(1000)
                .IsRequired();

            // Для типов decimal в EF Core необходимо явно указывать тип колонки и точность, 
            // иначе получим предупреждение при сборке или некорректное округление в БД.
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.Quantity)
                .IsRequired();

             // НАСТРОЙКА СВЯЗИ:
            // У каждого товара (HasOne) есть одна категория (Category).
            // У каждой категории (WithMany) может быть много товаров (Products).
            // Внешним ключом выступает свойство CategoryId (HasForeignKey).
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                // Запрещаем удаление категории, если к ней привязаны товары (Restrict).
                // Это защитит базу данных от появления "сиротских" товаров без категории.
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
