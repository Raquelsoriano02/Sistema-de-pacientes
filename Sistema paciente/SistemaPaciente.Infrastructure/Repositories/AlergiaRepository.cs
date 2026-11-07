using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Core;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Repositories
{
    public class AlergiaRepository : BaseRepository<Alergia>, IAlergiaRepository
    {
        public AlergiaRepository(SistemaPacienteContext context) : base(context)
        {
        }

        public async Task<Alergia> CreateAlergiaAsync(Alergia datos)
        {
            await ValidarAlergiaAsync(datos, 0);

            var alergia = new Alergia
            {
                PacienteId = datos.PacienteId,
                Sustancia = datos.Sustancia,
                Severidad = datos.Severidad,
                Observacion = datos.Observacion
            };

            return await Create(alergia);
        }

        public async Task<bool> UpdateAlergiaAsync(int id, Alergia datos)
        {
            var alergia = await GetById(id);
            if (alergia == null)
            {
                return false;
            }

            await ValidarAlergiaAsync(datos, id);

            alergia.PacienteId = datos.PacienteId;
            alergia.Sustancia = datos.Sustancia;
            alergia.Severidad = datos.Severidad;
            alergia.Observacion = datos.Observacion;

            await Update(alergia);
            return true;
        }

        public async Task<bool> DeleteAlergiaAsync(int id)
        {
            var alergia = await Delete(id);
            return alergia != null;
        }

        private async Task ValidarAlergiaAsync(Alergia datos, int id)
        {
            var pacienteExists = await _context.Pacientes.AnyAsync(p => p.Id == datos.PacienteId);
            if (!pacienteExists)
            {
                throw new AlergiaException($"El paciente con ID {datos.PacienteId} no existe");
            }

            var duplicada = await _context.Alergias.AnyAsync(a =>
                a.PacienteId == datos.PacienteId &&
                a.Sustancia == datos.Sustancia &&
                a.Id != id);

            if (duplicada)
            {
                throw new AlergiaException($"El paciente ya tiene registrada la alergia a {datos.Sustancia}");
            }
        }
    }
}
