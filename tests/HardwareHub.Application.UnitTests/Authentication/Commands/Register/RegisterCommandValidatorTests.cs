using FluentAssertions;
using FluentValidation.Results;
using HardwareHub.Application.Authentication.Commands.Register;
using Xunit;

namespace HardwareHub.Application.UnitTests.Authentication.Commands.Register
{
    public class RegisterCommandValidatorTests
    {
        private readonly RegisterCommandValidator _validator;

        public RegisterCommandValidatorTests()
        {
            // Инициализируем наш тестируемый валидатор перед каждым тестом
            _validator = new RegisterCommandValidator();
        }

        [Fact] // Атрибут xUnit указывает, что это единичный тест
        public void Validator_ShouldBeValid_WhenCommandIsCorrect()
        {
            // 1. Arrange (Подготовка)
            var command = new RegisterCommand("test@example.com", "Password123!", "testuser");

            // 2. Act (Действие)
            ValidationResult result = _validator.Validate(command);

            // 3. Assert (Проверка с помощью FluentAssertions)
            result.IsValid.Should().BeTrue("потому что все переданные данные полностью корректны");
        }

        [Theory] // Theory позволяет запускать один и тот же тест с разными входными параметрами
        [InlineData("")]            // Пустой email
        [InlineData("not-an-email")] // Некорректный формат email
        public void Validator_ShouldFail_WhenEmailIsInvalid(string invalidEmail)
        {
            // 1. Arrange
            var command = new RegisterCommand(invalidEmail, "Password123!", "testuser");

            // 2. Act
            ValidationResult result = _validator.Validate(command);

            // 3. Assert
            result.IsValid.Should().BeFalse("потому что передан невалидный Email");
            result.Errors.Should().ContainSingle(x => x.PropertyName == nameof(RegisterCommand.Email));
        }

        [Fact]
        public void Validator_ShouldFail_WhenPasswordIsTooShort()
        {
            // 1. Arrange
            var command = new RegisterCommand("test@example.com", "123", "testuser"); // Пароль 3 символа, а нужно минимум 6

            // 2. Act
            ValidationResult result = _validator.Validate(command);

            // 3. Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(RegisterCommand.Password))
                .Which.ErrorMessage.Should().Contain("Пароль должен быть не менее 6 символов.");
        }
    }
}
