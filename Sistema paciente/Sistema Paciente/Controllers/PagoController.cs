using SistemaPaciente.Application.Dtos.Pago;
using SistemaPaciente.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Sistema_Paciente.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IPagoService _service;

        public PagoController(IPagoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPagos()
        {
            var result = await _service.GetAllPagosAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPagoById(int id)
        {
            var result = await _service.GetPagoByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePago([FromBody] CreatePagoDto dto)
        {
            var result = await _service.CreatePagoAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetPagoById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePago(int id, [FromBody] UpdatePagoDto dto)
        {
            var result = await _service.UpdatePagoAsync(id, dto);
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
        public async Task<IActionResult> DeletePago(int id)
        {
            var result = await _service.DeletePagoAsync(id);
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
