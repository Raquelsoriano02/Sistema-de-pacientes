using SistemaPaciente.Domain.Entities;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface IMedicoRepository : IBaseRepository<Medico>
    {
        Task<Medico> CreateMedicoAsync(Medico datos);
        Task<bool> UpdateMedicoAsync(int id, Medico datos);
        Task<bool> DeleteMedicoAsync(int id);
    }
}
