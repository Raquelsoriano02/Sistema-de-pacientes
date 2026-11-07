using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Especialidad;

namespace SistemaPaciente.Application.Interfaces
{
    public interface IEspecialidadService
    {
        Task<ServiceResult<List<EspecialidadDto>>> GetAllEspecialidadesAsync();
        Task<ServiceResult<EspecialidadDto>> GetEspecialidadByIdAsync(int id);
        Task<ServiceResult<EspecialidadDto>> CreateEspecialidadAsync(CreateEspecialidadDto dto);
        Task<ServiceResult<bool>> UpdateEspecialidadAsync(int id, UpdateEspecialidadDto dto);
        Task<ServiceResult<bool>> DeleteEspecialidadAsync(int id);
    }
}
