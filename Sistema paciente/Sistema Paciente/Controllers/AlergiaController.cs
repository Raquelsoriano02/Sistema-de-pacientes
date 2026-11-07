using SistemaPaciente.Application.Dtos.Alergia;
using SistemaPaciente.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Sistema_Paciente.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlergiaController : ControllerBase
    {
        private readonly IAlergiaService _service;

        public AlergiaController(IAlergiaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAlergias()
        {
            var result = await _service.GetAllAlergiasAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlergiaById(int id)
        {
            var result = await _service.GetAlergiaByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlergia([FromBody] CreateAlergiaDto dto)
        {
            var result = await _service.CreateAlergiaAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetAlergiaById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlergia(int id, [FromBody] UpdateAlergiaDto dto)
        {
            var result = await _service.UpdateAlergiaAsync(id, dto);
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
        public async Task<IActionResult> DeleteAlergia(int id)
        {
            var result = await _service.DeleteAlergiaAsync(id);
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
