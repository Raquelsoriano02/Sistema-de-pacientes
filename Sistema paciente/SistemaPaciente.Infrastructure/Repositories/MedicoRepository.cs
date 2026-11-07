using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Core;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Repositories
{
    public class MedicoRepository : BaseRepository<Medico>, IMedicoRepository
    {
        public MedicoRepository(SistemaPacienteContext context) : base(context)
        {
        }

        public async Task<Medico> CreateMedicoAsync(Medico datos)
        {
            await ValidarMedicoAsync(datos, 0);

            var medico = new Medico
            {
                Nombre = datos.Nombre,
                Apellido = datos.Apellido,
                Email = datos.Email,
                Telefono = datos.Telefono,
                Licencia = datos.Licencia,
                EspecialidadId = datos.EspecialidadId,
                CostoConsulta = datos.CostoConsulta,
                Activo = datos.Activo
            };

            return await Create(medico);
        }

        public async Task<bool> UpdateMedicoAsync(int id, Medico datos)
        {
            var medico = await GetById(id);
            if (medico == null)
            {
                return false;
            }

            await ValidarMedicoAsync(datos, id);

            medico.Nombre = datos.Nombre;
            medico.Apellido = datos.Apellido;
            medico.Email = datos.Email;
            medico.Telefono = datos.Telefono;
            medico.Licencia = datos.Licencia;
            medico.EspecialidadId = datos.EspecialidadId;
            medico.CostoConsulta = datos.CostoConsulta;
            medico.Activo = datos.Activo;

            await Update(medico);
            return true;
        }

        public async Task<bool> DeleteMedicoAsync(int id)
        {
            var medico = await GetById(id);
            if (medico == null)
            {
                return false;
            }

            var citas = await _context.Citas.CountAsync(c => c.MedicoId == id);
            if (citas > 0)
            {
                throw new MedicoException($"No se puede eliminar el médico porque tiene {citas} cita(s) asociada(s)");
            }

            await Delete(id);
            return true;
        }

        private async Task ValidarMedicoAsync(Medico datos, int id)
        {
            var especialidad = await _context.Especialidades.FirstOrDefaultAsync(e => e.Id == datos.EspecialidadId);
            if (especialidad == null)
            {
                throw new MedicoException($"La especialidad con ID {datos.EspecialidadId} no existe");
            }

            if (!especialidad.Activa)
            {
                throw new MedicoException($"La especialidad {especialidad.Nombre} no está activa");
            }

            var licenciaExists = await _context.Medicos.AnyAsync(m => m.Licencia == datos.Licencia && m.Id != id);
            if (licenciaExists)
            {
                throw new MedicoException($"La licencia {datos.Licencia} ya está registrada");
            }

            if (!string.IsNullOrEmpty(datos.Email))
            {
                var emailExists = await _context.Medicos.AnyAsync(m => m.Email == datos.Email && m.Id != id);
                if (emailExists)
                {
                    throw new MedicoException($"El email {datos.Email} ya está registrado");
                }
            }
        }
    }
}
