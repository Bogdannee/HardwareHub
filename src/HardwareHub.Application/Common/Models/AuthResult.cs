namespace HardwareHub.Application.Common.Models
{
    /// <summary>
    /// Результат операции аутентификации/регистрации.
    /// Передается из слоя инфраструктуры в слой бизнес-логики.
    /// </summary>
    public class AuthResult
    {
        public bool IsSuccess { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public IEnumerable<string> Errors { get; private set; } = Array.Empty<string>();

        // Приватный конструктор для инкапсуляции
        private AuthResult() { }

        // Фабричный метод для успешного результата
        public static AuthResult Success(string token)
        {
            return new AuthResult 
            { 
                IsSuccess = true, 
                Token = token 
            };
        }

        // Фабричный метод для неуспешного результата с ошибками
        public static AuthResult Failure(IEnumerable<string> errors)
        {
            return new AuthResult 
            { 
                IsSuccess = false, 
                Errors = errors 
            };
        }
    }
}
