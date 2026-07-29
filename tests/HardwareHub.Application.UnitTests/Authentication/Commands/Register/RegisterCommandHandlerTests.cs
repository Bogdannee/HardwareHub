using FluentAssertions;
using HardwareHub.Application.Authentication.Commands.Register;
using HardwareHub.Application.Common.Interfaces;
using HardwareHub.Application.Common.Models;
using NSubstitute;
using Xunit;

namespace HardwareHub.Application.UnitTests.Authentication.Commands.Register
{
    public class RegisterCommandHandlerTests
    {
        private readonly IIdentityService _identityServiceMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            // 1. Arrange: Создаем фейковую (Mock) реализацию интерфейса с помощью NSubstitute
            _identityServiceMock = Substitute.For<IIdentityService>();
            
            // Внедряем заглушку в наш обработчик
            _handler = new RegisterCommandHandler(_identityServiceMock);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResultWithToken_WhenRegistrationIsSuccessful()
        {
            // 1. Arrange (Продолжение подготовки)
            var command = new RegisterCommand("newuser@example.com", "Password123!", "newuser");
            var expectedToken = "fake-jwt-token";
            
            // Настраиваем поведение заглушки: при вызове RegisterAsync с любыми параметрами вернуть успешный AuthResult
            _identityServiceMock
                .RegisterAsync(command.Email, command.Password, command.Username)
                .Returns(Task.FromResult(AuthResult.Success(expectedToken)));

            // 2. Act (Запускаем наш обработчик)
            var result = await _handler.Handle(command, CancellationToken.None);

            // 3. Assert (Проверяем результат)
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Token.Should().Be(expectedToken);

            // Дополнительная проверка от NSubstitute: убеждаемся, что наш обработчик действительно вызвал метод RegisterAsync у нашего сервиса ровно один раз
            await _identityServiceMock
                .Received(1)
                .RegisterAsync(command.Email, command.Password, command.Username);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenRegistrationFails()
        {
            // 1. Arrange
            var command = new RegisterCommand("existing@example.com", "Password123!", "existing");
            var errors = new[] { "Пользователь с таким Email уже существует." };

            _identityServiceMock
                .RegisterAsync(command.Email, command.Password, command.Username)
                .Returns(Task.FromResult(AuthResult.Failure(errors)));

            // 2. Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // 3. Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().BeEquivalentTo(errors);
        }
    }
}
