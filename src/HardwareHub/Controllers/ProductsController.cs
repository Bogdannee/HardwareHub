using System;
using System.Threading.Tasks;
using HardwareHub.Application.Catalog.Commands.CreateProduct;
using HardwareHub.Application.Catalog.Commands.DeleteProduct;
using HardwareHub.Application.Catalog.Commands.UpdateProduct;
using HardwareHub.Application.Catalog.Queries.GetProductById;
using HardwareHub.Application.Catalog.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HardwareHub.WebAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ISender _mediator;

        public ProductsController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Эндпоинт получения списка товаров с фильтрацией по категории и текстовым поиском.
        /// Параметры берутся из Query String (например: api/products?categoryId=GUID&searchTerm=видеокарта).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] Guid? categoryId, [FromQuery] string? searchTerm)
        {
            var query = new GetProductsQuery(categoryId, searchTerm);
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        /// <summary>
        /// Эндпоинт получения деталей товара по его Id.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));

            if (product == null)
            {
                return NotFound(new {Message = $"Товар с Id {id} не найден"});
            }

            return Ok(product);
        }

        /// <summary>
        /// Эндпоинт создания нового товара.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            try
            {
                var productId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = productId }, productId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message});
            }
        }

        /// <summary>
        /// Эндпоинт обновления товара по Id.
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { Error = "Идентификаторы товара в URL и в теле запроса не совпадают."});
            }

            try
            {
                var success = await _mediator.Send(command);

                if (!success)
                {
                    return NotFound(new { Message = $"Товар с Id {id} не найден"});
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message});
            }
        }

        /// <summary>
        /// Эндпоинт удаления товара по Id.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _mediator.Send(new DeleteProductCommand(id));

            if (!success)
            {
                return NotFound(new { Message = $"Товар с Id '{id}' не найден."});
            }

            return NoContent();
        }
    }
}