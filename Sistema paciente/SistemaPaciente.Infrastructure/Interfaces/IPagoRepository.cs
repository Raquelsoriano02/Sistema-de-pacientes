using SistemaPaciente.Domain.Entities;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface IPagoRepository : IBaseRepository<Pago>
    {
        Task<Pago> CreatePagoAsync(Pago datos);
        Task<bool> UpdatePagoAsync(int id, Pago datos);
        Task<bool> DeletePagoAsync(int id);
    }
}
