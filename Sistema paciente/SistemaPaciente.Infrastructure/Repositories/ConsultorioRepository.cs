using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Core;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Repositories
{
    public class ConsultorioRepository : BaseRepository<Consultorio>, IConsultorioRepository
    {
        public ConsultorioRepository(SistemaPacienteContext context) : base(context)
        {
        }

        public async Task<Consultorio> CreateConsultorioAsync(Consultorio datos)
        {
            await ValidarConsultorioAsync(datos, 0);

            var consultorio = new Consultorio
            {
                Codigo = datos.Codigo,
                Ubicacion = datos.Ubicacion,
                Activo = datos.Activo
            };

            return await Create(consultorio);
        }

        public async Task<bool> UpdateConsultorioAsync(int id, Consultorio datos)
        {
            var consultorio = await GetById(id);
            if (consultorio == null)
            {
                return false;
            }

            await ValidarConsultorioAsync(datos, id);

            consultorio.Codigo = datos.Codigo;
            consultorio.Ubicacion = datos.Ubicacion;
            consultorio.Activo = datos.Activo;

            await Update(consultorio);
            return true;
        }

        public async Task<bool> DeleteConsultorioAsync(int id)
        {
            var consultorio = await GetById(id);
            if (consultorio == null)
            {
                return false;
            }

            var citas = await _context.Citas.CountAsync(c => c.ConsultorioId == id);
            if (citas > 0)
            {
                throw new ConsultorioException($"No se puede eliminar el consultorio porque tiene {citas} cita(s) asociada(s)");
            }

            await Delete(id);
            return true;
        }

        private async Task ValidarConsultorioAsync(Consultorio datos, int id)
        {
            var codigoExists = await _context.Consultorios.AnyAsync(c => c.Codigo == datos.Codigo && c.Id != id);
            if (codigoExists)
            {
                throw new ConsultorioException($"El consultorio {datos.Codigo} ya está registrado");
            }
        }
    }
}
