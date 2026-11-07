using SistemaPaciente.Application.Dtos.Seguro;
using SistemaPaciente.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Sistema_Paciente.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguroController : ControllerBase
    {
        private readonly ISeguroService _service;

        public SeguroController(ISeguroService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSeguros()
        {
            var result = await _service.GetAllSegurosAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeguroById(int id)
        {
            var result = await _service.GetSeguroByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeguro([FromBody] CreateSeguroDto dto)
        {
            var result = await _service.CreateSeguroAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetSeguroById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeguro(int id, [FromBody] UpdateSeguroDto dto)
        {
            var result = await _service.UpdateSeguroAsync(id, dto);
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
        public async Task<IActionResult> DeleteSeguro(int id)
        {
            var result = await _service.DeleteSeguroAsync(id);
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
