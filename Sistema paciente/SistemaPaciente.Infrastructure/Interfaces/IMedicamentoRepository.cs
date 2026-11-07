using SistemaPaciente.Domain.Entities;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface IMedicamentoRepository : IBaseRepository<Medicamento>
    {
        Task<Medicamento> CreateMedicamentoAsync(Medicamento datos);
        Task<bool> UpdateMedicamentoAsync(int id, Medicamento datos);
        Task<bool> DeleteMedicamentoAsync(int id);
    }
}
