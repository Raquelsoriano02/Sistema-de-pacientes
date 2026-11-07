using SistemaPaciente.Application.Dtos.Receta;
using SistemaPaciente.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Sistema_Paciente.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecetaController : ControllerBase
    {
        private readonly IRecetaService _service;

        public RecetaController(IRecetaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecetas()
        {
            var result = await _service.GetAllRecetasAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecetaById(int id)
        {
            var result = await _service.GetRecetaByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReceta([FromBody] CreateRecetaDto dto)
        {
            var result = await _service.CreateRecetaAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetRecetaById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReceta(int id, [FromBody] UpdateRecetaDto dto)
        {
            var result = await _service.UpdateRecetaAsync(id, dto);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceta(int id)
        {
            var result = await _service.DeleteRecetaAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }
    }
}
