using SistemaPaciente.Domain.Entities;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface IConsultorioRepository : IBaseRepository<Consultorio>
    {
        Task<Consultorio> CreateConsultorioAsync(Consultorio datos);
        Task<bool> UpdateConsultorioAsync(int id, Consultorio datos);
        Task<bool> DeleteConsultorioAsync(int id);
    }
}
