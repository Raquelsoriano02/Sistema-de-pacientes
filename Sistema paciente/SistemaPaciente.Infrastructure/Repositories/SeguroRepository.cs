using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Core;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Repositories
{
    public class SeguroRepository : BaseRepository<Seguro>, ISeguroRepository
    {
        public SeguroRepository(SistemaPacienteContext context) : base(context)
        {
        }

        public async Task<Seguro> CreateSeguroAsync(Seguro datos)
        {
            await ValidarSeguroAsync(datos, 0);

            var seguro = new Seguro
            {
                Nombre = datos.Nombre,
                Telefono = datos.Telefono,
                PorcentajeCobertura = datos.PorcentajeCobertura,
                Activo = datos.Activo
            };

            return await Create(seguro);
        }

        public async Task<bool> UpdateSeguroAsync(int id, Seguro datos)
        {
            var seguro = await GetById(id);
            if (seguro == null)
            {
                return false;
            }

            await ValidarSeguroAsync(datos, id);

            seguro.Nombre = datos.Nombre;
            seguro.Telefono = datos.Telefono;
            seguro.PorcentajeCobertura = datos.PorcentajeCobertura;
            seguro.Activo = datos.Activo;

            await Update(seguro);
            return true;
        }

        public async Task<bool> DeleteSeguroAsync(int id)
        {
            var seguro = await GetById(id);
            if (seguro == null)
            {
                return false;
            }

            var pacientes = await _context.Pacientes.CountAsync(p => p.SeguroId == id);
            if (pacientes > 0)
            {
                throw new SeguroException($"No se puede eliminar el seguro porque tiene {pacientes} paciente(s) asociado(s)");
            }

            await Delete(id);
            return true;
        }

        private async Task ValidarSeguroAsync(Seguro datos, int id)
        {
            var nombreExists = await _context.Seguros.AnyAsync(s => s.Nombre == datos.Nombre && s.Id != id);
            if (nombreExists)
            {
                throw new SeguroException($"El seguro {datos.Nombre} ya está registrado");
            }
        }
    }
}
