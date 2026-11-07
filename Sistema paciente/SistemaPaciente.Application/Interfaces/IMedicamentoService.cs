using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Medicamento;

namespace SistemaPaciente.Application.Interfaces
{
    public interface IMedicamentoService
    {
        Task<ServiceResult<List<MedicamentoDto>>> GetAllMedicamentosAsync();
        Task<ServiceResult<MedicamentoDto>> GetMedicamentoByIdAsync(int id);
        Task<ServiceResult<MedicamentoDto>> CreateMedicamentoAsync(CreateMedicamentoDto dto);
        Task<ServiceResult<bool>> UpdateMedicamentoAsync(int id, UpdateMedicamentoDto dto);
        Task<ServiceResult<bool>> DeleteMedicamentoAsync(int id);
    }
}
