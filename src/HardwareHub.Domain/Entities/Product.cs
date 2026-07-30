using System;

namespace HardwareHub.Domain.Entities
{
    /// <summary>
    /// Доменная сущность Товара.
    /// </summary>
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        // Добавляем внешний ключ для связи с категорией.
        // Товар обязательно должен принадлежать какой-то одной категории.
        public Guid CategoryId { get; private set; }

        // Навигационное свойство для EF Core. 
        // Инициализируем "= null!" для подавления предупреждения компилятора о nullability.
        // EF Core автоматически заполнит это свойство при использовании .Include() в запросах.
        public Category Category { get; private set; } = null!;

        /// <summary>
        /// Приватный конструктор для Entity Framework Core и маппинга.
        /// </summary>
        private Product() { }

        /// <summary>
        /// Конструктор для создания нового товара. Теперь он требует обязательного указания CategoryId.
        /// </summary>
        public Product(string name, string description, decimal price, int quantity, Guid categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя товара не может быть пустым.", nameof(name));

            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной.", nameof(price));

            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным.", nameof(quantity));

            if (categoryId == Guid.Empty)
                throw new ArgumentException("Идентификатор категории должен быть указан.", nameof(categoryId));

            Id = Guid.NewGuid(); 
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            CategoryId = categoryId;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя товара не может быть пустым.", nameof(name));
            Name = name;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdatePrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentException("Цена не может быть отрицательной.");
            }
            Price = price;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Количество не может быть отрицательным.");
            }
            Quantity = quantity;
        }

        /// <summary>
        /// Метод изменения категории товара.
        /// </summary>
        public void UpdateCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new ArgumentException("Идентификатор категории должен быть указан.", nameof(categoryId));
            CategoryId = categoryId;
        }
    }
}