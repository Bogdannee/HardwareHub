using HardwareHub.Application.Catalog.Commands.CreateCategory;
using HardwareHub.Application.Catalog.Commands.DeleteCategory;
using HardwareHub.Application.Catalog.Commands.UpdateCategory;
using HardwareHub.Application.Catalog.Queries.GetCategories;
using HardwareHub.Application.Catalog.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HardwareHub.WebAPI.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ISender _mediator;

        public CategoriesController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Эндпоинт получения всех категорий.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            return Ok(categories);
        }
        /// <summary>
        /// Получить категорию по Id.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id));

            if (category == null)
            {
                return NotFound(new {Message = $"Категория с Id '{id}' не найдена."});  
            }

            return Ok(category);
        }

        /// <summary>
        /// Эндпоинт создания новой категории.
        /// Доступ к созданию категорий в будущем можно ограничить только для администраторов [Authorize(Roles = "Admin")].
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var categoryId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = categoryId}, categoryId);
        }

        /// <summary>
        /// Обновить категорию.
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { Error = "Идентификаторы в URL и теле запроса не совпадают." });
            }

            var success = await _mediator.Send(command);
            
            if (!success)
            {
                return NotFound(new { Message = $"Категория с Id '{id}' не найдена." });
            }

            return NoContent();
        }

        /// <summary>
        /// Удалить категорию.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _mediator.Send(new DeleteCategoryCommand(id));

                if (!success)
                {
                    return NotFound(new { Message = $"Категория с Id '{id}' не найдена." });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // Ловим бизнес-ошибку (например, если в категории есть товары)
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}