using SistemaPaciente.Domain.Entities;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface IAlergiaRepository : IBaseRepository<Alergia>
    {
        Task<Alergia> CreateAlergiaAsync(Alergia datos);
        Task<bool> UpdateAlergiaAsync(int id, Alergia datos);
        Task<bool> DeleteAlergiaAsync(int id);
    }
}
