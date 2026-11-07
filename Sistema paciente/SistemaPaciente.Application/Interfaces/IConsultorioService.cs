using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Consultorio;

namespace SistemaPaciente.Application.Interfaces
{
    public interface IConsultorioService
    {
        Task<ServiceResult<List<ConsultorioDto>>> GetAllConsultoriosAsync();
        Task<ServiceResult<ConsultorioDto>> GetConsultorioByIdAsync(int id);
        Task<ServiceResult<ConsultorioDto>> CreateConsultorioAsync(CreateConsultorioDto dto);
        Task<ServiceResult<bool>> UpdateConsultorioAsync(int id, UpdateConsultorioDto dto);
        Task<ServiceResult<bool>> DeleteConsultorioAsync(int id);
    }
}
