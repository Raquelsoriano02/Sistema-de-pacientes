using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Medico;

namespace SistemaPaciente.Application.Interfaces
{
    public interface IMedicoService
    {
        Task<ServiceResult<List<MedicoDto>>> GetAllMedicosAsync();
        Task<ServiceResult<MedicoDto>> GetMedicoByIdAsync(int id);
        Task<ServiceResult<MedicoDto>> CreateMedicoAsync(CreateMedicoDto dto);
        Task<ServiceResult<bool>> UpdateMedicoAsync(int id, UpdateMedicoDto dto);
        Task<ServiceResult<bool>> DeleteMedicoAsync(int id);
    }
}
