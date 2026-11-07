using SistemaPaciente.Domain.Entities;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface IConsultaRepository : IBaseRepository<Consulta>
    {
        Task<Consulta> CreateConsultaAsync(Consulta datos);
        Task<bool> UpdateConsultaAsync(int id, Consulta datos);
        Task<bool> DeleteConsultaAsync(int id);
    }
}
