using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Seguro;

namespace SistemaPaciente.Application.Interfaces
{
    public interface ISeguroService
    {
        Task<ServiceResult<List<SeguroDto>>> GetAllSegurosAsync();
        Task<ServiceResult<SeguroDto>> GetSeguroByIdAsync(int id);
        Task<ServiceResult<SeguroDto>> CreateSeguroAsync(CreateSeguroDto dto);
        Task<ServiceResult<bool>> UpdateSeguroAsync(int id, UpdateSeguroDto dto);
        Task<ServiceResult<bool>> DeleteSeguroAsync(int id);
    }
}
