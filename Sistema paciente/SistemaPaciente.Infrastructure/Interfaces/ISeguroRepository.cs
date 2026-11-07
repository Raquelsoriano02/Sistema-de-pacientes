using SistemaPaciente.Domain.Entities;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface ISeguroRepository : IBaseRepository<Seguro>
    {
        Task<Seguro> CreateSeguroAsync(Seguro datos);
        Task<bool> UpdateSeguroAsync(int id, Seguro datos);
        Task<bool> DeleteSeguroAsync(int id);
    }
}
