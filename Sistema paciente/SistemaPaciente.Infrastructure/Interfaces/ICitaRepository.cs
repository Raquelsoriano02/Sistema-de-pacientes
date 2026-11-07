using SistemaPaciente.Domain.Core;
using SistemaPaciente.Domain.Entities;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface ICitaRepository : IBaseRepository<Cita>
    {
        Task<List<DateTime>> GetDisponibilidadAsync(int medicoId, int consultorioId, DateOnly fecha);
        Task<bool> UpdateEstadoAsync(int id, EstadoCita estado);
        Task<Cita> CreateCitaAsync(Cita datos);
        Task<bool> UpdateCitaAsync(int id, Cita datos);
        Task<bool> DeleteCitaAsync(int id);
    }
}
