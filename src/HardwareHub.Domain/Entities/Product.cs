namespace HardwareHub.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        // Приватный конструктор для Entity Framework Core и маппинга
        private Product() { }

        public Product(string name, string description, decimal price, int quantity)
        {
            // Инициализация Id в конструкторе гарантирует, что каждая новая сущность будет иметь уникальный идентификатор
            // Guid.NewGuid() генерирует новый глобально уникальный идентификатор
            Id = Guid.NewGuid(); 
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        // Методы для изменения свойств (пример инкапсуляции бизнес-логики)
        public void UpdateName(string name)
        {
            // Здесь может быть бизнес-правило, например, проверка на уникальность имени
            Name = name;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdatePrice(decimal price)
        {
            // Пример валидации: цена не может быть отрицательной
            if (price < 0)
            {
                throw new ArgumentException("Цена не может быть отрицательной.");
            }
            Price = price;
        }

        public void UpdateQuantity(int quantity)
        {
            // Пример валидации: количество не может быть отрицательным
            if (quantity < 0)
            {
                throw new ArgumentException("Количество не может быть отрицательным.");
            }
            Quantity = quantity;
        }
    }
}