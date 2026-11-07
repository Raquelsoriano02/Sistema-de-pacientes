using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Infrastructure.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T?> GetById(int id);
        Task<List<T>> GetAll();
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<T?> Delete(int id);
    }
}
