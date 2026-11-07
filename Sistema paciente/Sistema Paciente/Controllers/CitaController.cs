using SistemaPaciente.Application.Dtos.Cita;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Core;
using Microsoft.AspNetCore.Mvc;

namespace Sistema_Paciente.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitaController : ControllerBase
    {
        private readonly ICitaService _service;

        public CitaController(ICitaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCitas()
        {
            var result = await _service.GetAllCitasAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCitaById(int id)
        {
            var result = await _service.GetCitaByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpGet("disponibilidad")]
        public async Task<IActionResult> GetDisponibilidad([FromQuery] int medicoId, [FromQuery] int consultorioId, [FromQuery] DateOnly fecha)
        {
            var result = await _service.GetDisponibilidadAsync(medicoId, consultorioId, fecha);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCita([FromBody] CreateCitaDto dto)
        {
            var result = await _service.CreateCitaAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetCitaById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCita(int id, [FromBody] UpdateCitaDto dto)
        {
            var result = await _service.UpdateCitaAsync(id, dto);
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

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> UpdateEstado(int id, [FromBody] EstadoCita estado)
        {
            var result = await _service.UpdateEstadoAsync(id, estado);
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
        public async Task<IActionResult> DeleteCita(int id)
        {
            var result = await _service.DeleteCitaAsync(id);
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
