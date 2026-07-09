using HardwareHub.Application.Common.Interfaces;
using HardwareHub.Application.Common.Models;
using MediatR;

namespace HardwareHub.Application.Authentication.Commands.Register
{
    // Запрос в MediatR: описывает входные данные для регистрации и тип возвращаемого значения (AuthResult)
    public record RegisterCommand(
        string Email,
        string Password,
        string Username) : IRequest<AuthResult>;

    // Обработчик команды: содержит бизнес-логику обработки этого запроса
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
    {
        private readonly IIdentityService _identityService;

        public RegisterCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Просто делегируем выполнение нашему сервису аутентификации
            return await _identityService.RegisterAsync(request.Email, request.Password, request.Username);
        }
    }
}
