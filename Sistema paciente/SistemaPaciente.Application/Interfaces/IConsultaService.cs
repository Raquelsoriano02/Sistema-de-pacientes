using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Consulta;

namespace SistemaPaciente.Application.Interfaces
{
    public interface IConsultaService
    {
        Task<ServiceResult<List<ConsultaDto>>> GetAllConsultasAsync();
        Task<ServiceResult<ConsultaDto>> GetConsultaByIdAsync(int id);
        Task<ServiceResult<ConsultaDto>> CreateConsultaAsync(CreateConsultaDto dto);
        Task<ServiceResult<bool>> UpdateConsultaAsync(int id, UpdateConsultaDto dto);
        Task<ServiceResult<bool>> DeleteConsultaAsync(int id);
    }
}
