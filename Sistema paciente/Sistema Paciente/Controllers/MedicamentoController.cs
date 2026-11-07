using SistemaPaciente.Application.Dtos.Medicamento;
using SistemaPaciente.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Sistema_Paciente.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicamentoController : ControllerBase
    {
        private readonly IMedicamentoService _service;

        public MedicamentoController(IMedicamentoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMedicamentos()
        {
            var result = await _service.GetAllMedicamentosAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicamentoById(int id)
        {
            var result = await _service.GetMedicamentoByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicamento([FromBody] CreateMedicamentoDto dto)
        {
            var result = await _service.CreateMedicamentoAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetMedicamentoById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicamento(int id, [FromBody] UpdateMedicamentoDto dto)
        {
            var result = await _service.UpdateMedicamentoAsync(id, dto);
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
        public async Task<IActionResult> DeleteMedicamento(int id)
        {
            var result = await _service.DeleteMedicamentoAsync(id);
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
