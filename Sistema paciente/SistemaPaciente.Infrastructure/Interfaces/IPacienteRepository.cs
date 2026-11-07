using SistemaPaciente.Domain.Entities;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface IPacienteRepository : IBaseRepository<Paciente>
    {
        Task<Paciente> CreatePacienteAsync(Paciente datos);
        Task<bool> UpdatePacienteAsync(int id, Paciente datos);
        Task<bool> DeletePacienteAsync(int id);
    }
}
