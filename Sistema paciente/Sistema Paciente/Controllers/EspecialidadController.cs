using SistemaPaciente.Application.Dtos.Especialidad;
using SistemaPaciente.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Sistema_Paciente.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadController : ControllerBase
    {
        private readonly IEspecialidadService _service;

        public EspecialidadController(IEspecialidadService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEspecialidades()
        {
            var result = await _service.GetAllEspecialidadesAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEspecialidadById(int id)
        {
            var result = await _service.GetEspecialidadByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEspecialidad([FromBody] CreateEspecialidadDto dto)
        {
            var result = await _service.CreateEspecialidadAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetEspecialidadById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEspecialidad(int id, [FromBody] UpdateEspecialidadDto dto)
        {
            var result = await _service.UpdateEspecialidadAsync(id, dto);
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
        public async Task<IActionResult> DeleteEspecialidad(int id)
        {
            var result = await _service.DeleteEspecialidadAsync(id);
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
