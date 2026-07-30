using HardwareHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HardwareHub.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Конфигурация Fluent API для сущности Category.
    /// Задает правила отображения класса Category на таблицу базы данных.
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasMaxLength(500)
                .IsRequired(false);
        }
    }
}