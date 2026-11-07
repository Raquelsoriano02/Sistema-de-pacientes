using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Pago;

namespace SistemaPaciente.Application.Interfaces
{
    public interface IPagoService
    {
        Task<ServiceResult<List<PagoDto>>> GetAllPagosAsync();
        Task<ServiceResult<PagoDto>> GetPagoByIdAsync(int id);
        Task<ServiceResult<PagoDto>> CreatePagoAsync(CreatePagoDto dto);
        Task<ServiceResult<bool>> UpdatePagoAsync(int id, UpdatePagoDto dto);
        Task<ServiceResult<bool>> DeletePagoAsync(int id);
    }
}
