using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Alergia;

namespace SistemaPaciente.Application.Interfaces
{
    public interface IAlergiaService
    {
        Task<ServiceResult<List<AlergiaDto>>> GetAllAlergiasAsync();
        Task<ServiceResult<AlergiaDto>> GetAlergiaByIdAsync(int id);
        Task<ServiceResult<AlergiaDto>> CreateAlergiaAsync(CreateAlergiaDto dto);
        Task<ServiceResult<bool>> UpdateAlergiaAsync(int id, UpdateAlergiaDto dto);
        Task<ServiceResult<bool>> DeleteAlergiaAsync(int id);
    }
}
