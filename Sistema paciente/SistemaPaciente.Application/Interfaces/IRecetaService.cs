using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Receta;

namespace SistemaPaciente.Application.Interfaces
{
    public interface IRecetaService
    {
        Task<ServiceResult<List<RecetaDto>>> GetAllRecetasAsync();
        Task<ServiceResult<RecetaDto>> GetRecetaByIdAsync(int id);
        Task<ServiceResult<RecetaDto>> CreateRecetaAsync(CreateRecetaDto dto);
        Task<ServiceResult<bool>> UpdateRecetaAsync(int id, UpdateRecetaDto dto);
        Task<ServiceResult<bool>> DeleteRecetaAsync(int id);
    }
}
