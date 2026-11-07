using SistemaPaciente.Domain.Entities;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface IHorarioRepository : IBaseRepository<Horario>
    {
        Task<Horario> CreateHorarioAsync(Horario datos);
        Task<bool> UpdateHorarioAsync(int id, Horario datos);
        Task<bool> DeleteHorarioAsync(int id);
    }
}
