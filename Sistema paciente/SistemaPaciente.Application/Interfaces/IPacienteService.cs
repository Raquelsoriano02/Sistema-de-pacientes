using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Paciente;

namespace SistemaPaciente.Application.Interfaces
{
    public interface IPacienteService
    {
        Task<ServiceResult<List<PacienteDto>>> GetAllPacientesAsync();
        Task<ServiceResult<PacienteDto>> GetPacienteByIdAsync(int id);
        Task<ServiceResult<PacienteDto>> CreatePacienteAsync(CreatePacienteDto dto);
        Task<ServiceResult<bool>> UpdatePacienteAsync(int id, UpdatePacienteDto dto);
        Task<ServiceResult<bool>> DeletePacienteAsync(int id);
    }
}
