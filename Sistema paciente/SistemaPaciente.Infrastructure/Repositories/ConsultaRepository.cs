using SistemaPaciente.Domain.Core;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Core;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Repositories
{
    public class ConsultaRepository : BaseRepository<Consulta>, IConsultaRepository
    {
        public ConsultaRepository(SistemaPacienteContext context) : base(context)
        {
        }

        public async Task<Consulta> CreateConsultaAsync(Consulta datos)
        {
            await using var transaccion = await _context.Database.BeginTransactionAsync();

            var cita = await _context.Citas.FirstOrDefaultAsync(c => c.Id == datos.CitaId);
            if (cita == null)
            {
                throw new ConsultaException($"La cita con ID {datos.CitaId} no existe");
            }

            if (cita.Estado == EstadoCita.Cancelada)
            {
                throw new ConsultaException("No se puede registrar una consulta en una cita cancelada");
            }

            var consultaExists = await _context.Consultas.AnyAsync(c => c.CitaId == datos.CitaId);
            if (consultaExists)
            {
                throw new ConsultaException("La cita ya tiene una consulta registrada");
            }

            var consulta = new Consulta
            {
                CitaId = datos.CitaId,
                PesoKg = datos.PesoKg,
                TemperaturaC = datos.TemperaturaC,
                PresionArterial = datos.PresionArterial,
                Sintomas = datos.Sintomas,
                Diagnostico = datos.Diagnostico,
                Tratamiento = datos.Tratamiento
            };

            cita.Estado = EstadoCita.Completada;

            await Create(consulta);
            await transaccion.CommitAsync();

            return consulta;
        }

        public async Task<bool> UpdateConsultaAsync(int id, Consulta datos)
        {
            var consulta = await GetById(id);
            if (consulta == null)
            {
                return false;
            }

            if (consulta.CitaId != datos.CitaId)
            {
                throw new ConsultaException("No se puede cambiar la cita de una consulta");
            }

            consulta.PesoKg = datos.PesoKg;
            consulta.TemperaturaC = datos.TemperaturaC;
            consulta.PresionArterial = datos.PresionArterial;
            consulta.Sintomas = datos.Sintomas;
            consulta.Diagnostico = datos.Diagnostico;
            consulta.Tratamiento = datos.Tratamiento;

            await Update(consulta);
            return true;
        }

        public async Task<bool> DeleteConsultaAsync(int id)
        {
            await using var transaccion = await _context.Database.BeginTransactionAsync();

            var consulta = await GetById(id);
            if (consulta == null)
            {
                return false;
            }

            var recetas = await _context.Recetas.CountAsync(r => r.ConsultaId == id);
            if (recetas > 0)
            {
                throw new ConsultaException($"No se puede eliminar la consulta porque tiene {recetas} receta(s) asociada(s)");
            }

            var cita = await _context.Citas.FirstAsync(c => c.Id == consulta.CitaId);
            cita.Estado = EstadoCita.Confirmada;

            await Delete(id);
            await transaccion.CommitAsync();

            return true;
        }
    }
}
