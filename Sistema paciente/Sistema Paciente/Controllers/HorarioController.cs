using SistemaPaciente.Application.Dtos.Horario;
using SistemaPaciente.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Sistema_Paciente.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioController : ControllerBase
    {
        private readonly IHorarioService _service;

        public HorarioController(IHorarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHorarios()
        {
            var result = await _service.GetAllHorariosAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHorarioById(int id)
        {
            var result = await _service.GetHorarioByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHorario([FromBody] CreateHorarioDto dto)
        {
            var result = await _service.CreateHorarioAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetHorarioById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHorario(int id, [FromBody] UpdateHorarioDto dto)
        {
            var result = await _service.UpdateHorarioAsync(id, dto);
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
        public async Task<IActionResult> DeleteHorario(int id)
        {
            var result = await _service.DeleteHorarioAsync(id);
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
