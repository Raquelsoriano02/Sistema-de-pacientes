using SistemaPaciente.Application.Dtos.Consultorio;
using SistemaPaciente.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Sistema_Paciente.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultorioController : ControllerBase
    {
        private readonly IConsultorioService _service;

        public ConsultorioController(IConsultorioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConsultorios()
        {
            var result = await _service.GetAllConsultoriosAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConsultorioById(int id)
        {
            var result = await _service.GetConsultorioByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateConsultorio([FromBody] CreateConsultorioDto dto)
        {
            var result = await _service.CreateConsultorioAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetConsultorioById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsultorio(int id, [FromBody] UpdateConsultorioDto dto)
        {
            var result = await _service.UpdateConsultorioAsync(id, dto);
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
        public async Task<IActionResult> DeleteConsultorio(int id)
        {
            var result = await _service.DeleteConsultorioAsync(id);
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
