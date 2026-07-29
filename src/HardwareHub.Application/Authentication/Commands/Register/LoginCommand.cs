using HardwareHub.Application.Common.Interfaces;
using HardwareHub.Application.Common.Models;
using MediatR;

namespace HardwareHub.Application.Authentication.Commands.Login
{
    public record LoginCommand(
        string Email,
        string Password) : IRequest<AuthResult>;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.LoginAsync(request.Email, request.Password);
        }
    }
}
