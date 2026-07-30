using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HardwareHub.Domain.Entities
{
    /// <summary>
    /// Доменная сущность Категории товаров.
    /// Представляет группу, к которой относится товар (например, "Видеокарты", "Процессоры").
    /// </summary>
    public class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        private readonly List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

        /// <summary>
        /// Конструктор без параметров необходим для работы ORM Entity Framework Core.
        /// EF Core использует рефлексию для создания объектов при выборке данных из БД.
        /// </summary>
        private Category() { }

        /// <summary>
        /// Публичный конструктор для создания новой категории через бизнес-логику.
        /// </summary>
        public Category(string name, string description)
        {
            // Валидация инвариантов на уровне домена (базовая защита от критических ошибок)
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Название категории не может быть пустым.", nameof(name));
            }

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Метод для безопасного обновления названия категории.
        /// </summary>
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Название категории не может быть пустым.", nameof(name));
            }
            Name = name;
        }

        /// <summary>
        /// Метод для безопасного обновления описания категории.
        /// </summary>
        public void UpdateDescription(string description)
        {
            Description = description;
        }
    }
}