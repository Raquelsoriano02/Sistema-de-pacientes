using SistemaPaciente.Domain.Entities;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface IRecetaRepository : IBaseRepository<Receta>
    {
        Task<Receta> CreateRecetaAsync(Receta datos);
        Task<bool> UpdateRecetaAsync(int id, Receta datos);
        Task<bool> DeleteRecetaAsync(int id);
    }
}
