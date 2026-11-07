using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Cita;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Application.Interfaces
{
    public interface ICitaService
    {
        Task<ServiceResult<List<CitaDto>>> GetAllCitasAsync();
        Task<ServiceResult<CitaDto>> GetCitaByIdAsync(int id);
        Task<ServiceResult<List<DateTime>>> GetDisponibilidadAsync(int medicoId, int consultorioId, DateOnly fecha);
        Task<ServiceResult<bool>> UpdateEstadoAsync(int id, EstadoCita estado);
        Task<ServiceResult<CitaDto>> CreateCitaAsync(CreateCitaDto dto);
        Task<ServiceResult<bool>> UpdateCitaAsync(int id, UpdateCitaDto dto);
        Task<ServiceResult<bool>> DeleteCitaAsync(int id);
    }
}
