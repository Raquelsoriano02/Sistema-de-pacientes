using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Core;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Repositories
{
    public class MedicamentoRepository : BaseRepository<Medicamento>, IMedicamentoRepository
    {
        public MedicamentoRepository(SistemaPacienteContext context) : base(context)
        {
        }

        public async Task<Medicamento> CreateMedicamentoAsync(Medicamento datos)
        {
            await ValidarMedicamentoAsync(datos, 0);

            var medicamento = new Medicamento
            {
                Nombre = datos.Nombre,
                PrincipioActivo = datos.PrincipioActivo,
                Presentacion = datos.Presentacion,
                Activo = datos.Activo
            };

            return await Create(medicamento);
        }

        public async Task<bool> UpdateMedicamentoAsync(int id, Medicamento datos)
        {
            var medicamento = await GetById(id);
            if (medicamento == null)
            {
                return false;
            }

            await ValidarMedicamentoAsync(datos, id);

            medicamento.Nombre = datos.Nombre;
            medicamento.PrincipioActivo = datos.PrincipioActivo;
            medicamento.Presentacion = datos.Presentacion;
            medicamento.Activo = datos.Activo;

            await Update(medicamento);
            return true;
        }

        public async Task<bool> DeleteMedicamentoAsync(int id)
        {
            var medicamento = await GetById(id);
            if (medicamento == null)
            {
                return false;
            }

            var recetas = await _context.Recetas.CountAsync(r => r.MedicamentoId == id);
            if (recetas > 0)
            {
                throw new MedicamentoException($"No se puede eliminar el medicamento porque tiene {recetas} receta(s) asociada(s)");
            }

            await Delete(id);
            return true;
        }

        private async Task ValidarMedicamentoAsync(Medicamento datos, int id)
        {
            var nombreExists = await _context.Medicamentos.AnyAsync(m => m.Nombre == datos.Nombre && m.Id != id);
            if (nombreExists)
            {
                throw new MedicamentoException($"El medicamento {datos.Nombre} ya está registrado");
            }
        }
    }
}
