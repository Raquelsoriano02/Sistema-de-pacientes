using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Core;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Repositories
{
    public class EspecialidadRepository : BaseRepository<Especialidad>, IEspecialidadRepository
    {
        public EspecialidadRepository(SistemaPacienteContext context) : base(context)
        {
        }

        public async Task<Especialidad> CreateEspecialidadAsync(Especialidad datos)
        {
            await ValidarEspecialidadAsync(datos, 0);

            var especialidad = new Especialidad
            {
                Nombre = datos.Nombre,
                Descripcion = datos.Descripcion,
                Activa = datos.Activa
            };

            return await Create(especialidad);
        }

        public async Task<bool> UpdateEspecialidadAsync(int id, Especialidad datos)
        {
            var especialidad = await GetById(id);
            if (especialidad == null)
            {
                return false;
            }

            await ValidarEspecialidadAsync(datos, id);

            especialidad.Nombre = datos.Nombre;
            especialidad.Descripcion = datos.Descripcion;
            especialidad.Activa = datos.Activa;

            await Update(especialidad);
            return true;
        }

        public async Task<bool> DeleteEspecialidadAsync(int id)
        {
            var especialidad = await GetById(id);
            if (especialidad == null)
            {
                return false;
            }

            var medicos = await _context.Medicos.CountAsync(m => m.EspecialidadId == id);
            if (medicos > 0)
            {
                throw new EspecialidadException($"No se puede eliminar la especialidad porque tiene {medicos} médico(s) asociado(s)");
            }

            await Delete(id);
            return true;
        }

        private async Task ValidarEspecialidadAsync(Especialidad datos, int id)
        {
            var nombreExists = await _context.Especialidades.AnyAsync(e => e.Nombre == datos.Nombre && e.Id != id);
            if (nombreExists)
            {
                throw new EspecialidadException($"La especialidad {datos.Nombre} ya está registrada");
            }
        }
    }
}
