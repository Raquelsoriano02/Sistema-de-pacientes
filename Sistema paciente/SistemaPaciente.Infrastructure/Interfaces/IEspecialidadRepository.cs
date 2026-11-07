using SistemaPaciente.Domain.Entities;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface IEspecialidadRepository : IBaseRepository<Especialidad>
    {
        Task<Especialidad> CreateEspecialidadAsync(Especialidad datos);
        Task<bool> UpdateEspecialidadAsync(int id, Especialidad datos);
        Task<bool> DeleteEspecialidadAsync(int id);
    }
}
