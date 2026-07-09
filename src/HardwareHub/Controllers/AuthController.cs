using HardwareHub.Application.Authentication.Commands.Login;
using HardwareHub.Application.Authentication.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HardwareHub.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _mediator;

        // Внедряем ISender (часть интерфейса IMediator, отвечающая только за отправку сообщений).
        // Это более узкий и чистый интерфейс по сравнению с полным IMediator.
        public AuthController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Эндпоинт регистрации нового пользователя.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            // Отправляем команду в MediatR. Он автоматически запустит валидаторы FluentValidation,
            // а затем вызовет обработчик RegisterCommandHandler.
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                // Если возникли ошибки, возвращаем 400 Bad Request со списком ошибок
                return BadRequest(new { Errors = result.Errors });
            }

            // В случае успеха возвращаем 200 OK и JWT-токен
            return Ok(new { Token = result.Token });
        }

        /// <summary>
        /// Эндпоинт авторизации пользователя.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Errors = result.Errors });
            }

            return Ok(new { Token = result.Token });
        }
    }
}
