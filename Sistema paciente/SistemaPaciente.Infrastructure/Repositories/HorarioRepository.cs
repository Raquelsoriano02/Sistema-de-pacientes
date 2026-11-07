using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Core;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Repositories
{
    public class HorarioRepository : BaseRepository<Horario>, IHorarioRepository
    {
        public HorarioRepository(SistemaPacienteContext context) : base(context)
        {
        }

        public async Task<Horario> CreateHorarioAsync(Horario datos)
        {
            await ValidarHorarioAsync(datos, 0);

            var horario = new Horario
            {
                MedicoId = datos.MedicoId,
                DiaSemana = datos.DiaSemana,
                HoraInicio = datos.HoraInicio,
                HoraFin = datos.HoraFin,
                Disponible = datos.Disponible
            };

            return await Create(horario);
        }

        public async Task<bool> UpdateHorarioAsync(int id, Horario datos)
        {
            var horario = await GetById(id);
            if (horario == null)
            {
                return false;
            }

            await ValidarHorarioAsync(datos, id);

            horario.MedicoId = datos.MedicoId;
            horario.DiaSemana = datos.DiaSemana;
            horario.HoraInicio = datos.HoraInicio;
            horario.HoraFin = datos.HoraFin;
            horario.Disponible = datos.Disponible;

            await Update(horario);
            return true;
        }

        public async Task<bool> DeleteHorarioAsync(int id)
        {
            var horario = await Delete(id);
            return horario != null;
        }

        private async Task ValidarHorarioAsync(Horario datos, int id)
        {
            if (datos.HoraInicio >= datos.HoraFin)
            {
                throw new HorarioException("La hora de inicio debe ser menor que la hora de fin");
            }

            var medicoExists = await _context.Medicos.AnyAsync(m => m.Id == datos.MedicoId);
            if (!medicoExists)
            {
                throw new HorarioException($"El médico con ID {datos.MedicoId} no existe");
            }

            var cruce = await _context.Horarios.AnyAsync(h =>
                h.Id != id &&
                h.MedicoId == datos.MedicoId &&
                h.DiaSemana == datos.DiaSemana &&
                h.HoraInicio < datos.HoraFin &&
                h.HoraFin > datos.HoraInicio);

            if (cruce)
            {
                throw new HorarioException("El horario se cruza con otro horario del médico");
            }
        }
    }
}
