using System.Data;
using SistemaPaciente.Domain.Core;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Core;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Repositories
{
    public class CitaRepository : BaseRepository<Cita>, ICitaRepository
    {
        private const int DuracionMinutos = 30;
        private const int ErrorDeadlock = 1205;

        public CitaRepository(SistemaPacienteContext context) : base(context)
        {
        }

        public async Task<List<DateTime>> GetDisponibilidadAsync(int medicoId, int consultorioId, DateOnly fecha)
        {
            await ObtenerMedicoActivoAsync(medicoId);
            await ValidarConsultorioActivoAsync(consultorioId);

            var dia = (DiaSemana)fecha.DayOfWeek;
            var inicioDia = fecha.ToDateTime(TimeOnly.MinValue);
            var finDia = inicioDia.AddDays(1);

            var horarios = await _context.Horarios
                .Where(h => h.MedicoId == medicoId && h.DiaSemana == dia && h.Disponible)
                .OrderBy(h => h.HoraInicio)
                .ToListAsync();

            var citas = await _context.Citas
                .Where(c => (c.MedicoId == medicoId || c.ConsultorioId == consultorioId)
                    && c.Estado != EstadoCita.Cancelada
                    && c.FechaHoraInicio < finDia
                    && c.FechaHoraFin > inicioDia)
                .ToListAsync();

            var disponibles = new List<DateTime>();

            foreach (var horario in horarios)
            {
                var inicio = fecha.ToDateTime(horario.HoraInicio);
                var cierre = fecha.ToDateTime(horario.HoraFin);

                while (inicio.AddMinutes(DuracionMinutos) <= cierre)
                {
                    var fin = inicio.AddMinutes(DuracionMinutos);
                    var ocupado = citas.Any(c => c.FechaHoraInicio < fin && c.FechaHoraFin > inicio);

                    if (inicio > DateTime.Now && !ocupado)
                    {
                        disponibles.Add(inicio);
                    }

                    inicio = fin;
                }
            }

            return disponibles;
        }

        public async Task<Cita> CreateCitaAsync(Cita datos)
        {
            var cita = new Cita();

            await EjecutarEnTransaccionAsync(async () =>
            {
                var inicio = NormalizarFecha(datos.FechaHoraInicio);
                var medico = await ValidarCitaAsync(datos, inicio, 0);

                cita.PacienteId = datos.PacienteId;
                cita.MedicoId = datos.MedicoId;
                cita.ConsultorioId = datos.ConsultorioId;
                cita.FechaHoraInicio = inicio;
                cita.FechaHoraFin = inicio.AddMinutes(DuracionMinutos);
                cita.Costo = medico.CostoConsulta;
                cita.Cobertura = await CalcularCoberturaAsync(datos.PacienteId, medico.CostoConsulta);
                cita.Estado = EstadoCita.Pendiente;
                cita.Motivo = datos.Motivo;

                await Create(cita);
            });

            return cita;
        }

        public async Task<bool> UpdateCitaAsync(int id, Cita datos)
        {
            var encontrado = true;

            await EjecutarEnTransaccionAsync(async () =>
            {
                var cita = await GetById(id);
                if (cita == null)
                {
                    encontrado = false;
                    return;
                }

                if (cita.Estado == EstadoCita.Completada || cita.Estado == EstadoCita.Cancelada)
                {
                    throw new CitaException($"No se puede modificar una cita {cita.Estado}");
                }

                var inicio = NormalizarFecha(datos.FechaHoraInicio);
                var medico = await ValidarCitaAsync(datos, inicio, id);

                if (cita.MedicoId != datos.MedicoId || cita.PacienteId != datos.PacienteId)
                {
                    var costo = medico.CostoConsulta;
                    var cobertura = await CalcularCoberturaAsync(datos.PacienteId, costo);
                    var pagado = await _context.Pagos
                        .Where(p => p.CitaId == id)
                        .SumAsync(p => p.Monto);

                    if (pagado > costo - cobertura)
                    {
                        throw new CitaException($"La cita ya tiene pagado {pagado} y el nuevo total a pagar es {costo - cobertura}");
                    }

                    cita.Costo = costo;
                    cita.Cobertura = cobertura;
                }

                cita.PacienteId = datos.PacienteId;
                cita.MedicoId = datos.MedicoId;
                cita.ConsultorioId = datos.ConsultorioId;
                cita.FechaHoraInicio = inicio;
                cita.FechaHoraFin = inicio.AddMinutes(DuracionMinutos);
                cita.Motivo = datos.Motivo;

                await Update(cita);
            });

            return encontrado;
        }

        public async Task<bool> UpdateEstadoAsync(int id, EstadoCita estado)
        {
            var cita = await GetById(id);
            if (cita == null)
            {
                return false;
            }

            if (cita.Estado == EstadoCita.Completada || cita.Estado == EstadoCita.Cancelada)
            {
                throw new CitaException($"No se puede cambiar el estado de una cita {cita.Estado}");
            }

            if (estado == EstadoCita.Completada)
            {
                throw new CitaException("La cita se completa al registrar su consulta");
            }

            if (estado == EstadoCita.Cancelada)
            {
                var tienePagos = await _context.Pagos.AnyAsync(p => p.CitaId == id);
                if (tienePagos)
                {
                    throw new CitaException("No se puede cancelar una cita con pagos registrados");
                }
            }

            cita.Estado = estado;
            await Update(cita);
            return true;
        }

        public async Task<bool> DeleteCitaAsync(int id)
        {
            var cita = await GetById(id);
            if (cita == null)
            {
                return false;
            }

            var pagos = await _context.Pagos.CountAsync(p => p.CitaId == id);
            if (pagos > 0)
            {
                throw new CitaException($"No se puede eliminar la cita porque tiene {pagos} pago(s) asociado(s)");
            }

            var tieneConsulta = await _context.Consultas.AnyAsync(c => c.CitaId == id);
            if (tieneConsulta)
            {
                throw new CitaException("No se puede eliminar la cita porque tiene una consulta asociada");
            }

            await Delete(id);
            return true;
        }

        private async Task<Medico> ValidarCitaAsync(Cita datos, DateTime inicio, int id)
        {
            var pacienteExists = await _context.Pacientes.AnyAsync(p => p.Id == datos.PacienteId);
            if (!pacienteExists)
            {
                throw new CitaException($"El paciente con ID {datos.PacienteId} no existe");
            }

            var medico = await ObtenerMedicoActivoAsync(datos.MedicoId);
            await ValidarConsultorioActivoAsync(datos.ConsultorioId);

            if (inicio <= DateTime.Now)
            {
                throw new CitaException("La fecha de la cita debe ser futura");
            }

            var fin = inicio.AddMinutes(DuracionMinutos);
            var dia = (DiaSemana)inicio.DayOfWeek;
            var horaInicio = TimeOnly.FromDateTime(inicio);
            var horaFin = TimeOnly.FromDateTime(fin);

            var enHorario = fin.Date == inicio.Date && await _context.Horarios.AnyAsync(h =>
                h.MedicoId == datos.MedicoId &&
                h.DiaSemana == dia &&
                h.Disponible &&
                h.HoraInicio <= horaInicio &&
                h.HoraFin >= horaFin);

            if (!enHorario)
            {
                throw new CitaException("El médico no tiene disponibilidad en ese horario");
            }

            var medicoOcupado = await _context.Citas.AnyAsync(c =>
                c.Id != id &&
                c.MedicoId == datos.MedicoId &&
                c.Estado != EstadoCita.Cancelada &&
                c.FechaHoraInicio < fin &&
                c.FechaHoraFin > inicio);

            if (medicoOcupado)
            {
                throw new CitaException("El médico ya tiene una cita en ese horario");
            }

            var consultorioOcupado = await _context.Citas.AnyAsync(c =>
                c.Id != id &&
                c.ConsultorioId == datos.ConsultorioId &&
                c.Estado != EstadoCita.Cancelada &&
                c.FechaHoraInicio < fin &&
                c.FechaHoraFin > inicio);

            if (consultorioOcupado)
            {
                throw new CitaException("El consultorio ya está ocupado en ese horario");
            }

            var pacienteOcupado = await _context.Citas.AnyAsync(c =>
                c.Id != id &&
                c.PacienteId == datos.PacienteId &&
                c.Estado != EstadoCita.Cancelada &&
                c.FechaHoraInicio < fin &&
                c.FechaHoraFin > inicio);

            if (pacienteOcupado)
            {
                throw new CitaException("El paciente ya tiene una cita en ese horario");
            }

            return medico;
        }

        private async Task<decimal> CalcularCoberturaAsync(int pacienteId, decimal costo)
        {
            var porcentaje = await _context.Pacientes
                .Where(p => p.Id == pacienteId && p.Seguro != null && p.Seguro.Activo)
                .Select(p => p.Seguro!.PorcentajeCobertura)
                .FirstOrDefaultAsync();

            return Math.Round(costo * porcentaje / 100, 2);
        }

        private async Task<Medico> ObtenerMedicoActivoAsync(int medicoId)
        {
            var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.Id == medicoId);
            if (medico == null)
            {
                throw new CitaException($"El médico con ID {medicoId} no existe");
            }

            if (!medico.Activo)
            {
                throw new CitaException($"El médico con ID {medicoId} no está activo");
            }

            return medico;
        }

        private async Task ValidarConsultorioActivoAsync(int consultorioId)
        {
            var consultorio = await _context.Consultorios.FirstOrDefaultAsync(c => c.Id == consultorioId);
            if (consultorio == null)
            {
                throw new CitaException($"El consultorio con ID {consultorioId} no existe");
            }

            if (!consultorio.Activo)
            {
                throw new CitaException($"El consultorio con ID {consultorioId} no está activo");
            }
        }

        private async Task EjecutarEnTransaccionAsync(Func<Task> accion)
        {
            await using var transaccion = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                await accion();
                await transaccion.CommitAsync();
            }
            catch (Exception ex) when (ex.GetBaseException() is SqlException { Number: ErrorDeadlock })
            {
                throw new CitaException("Ese horario está siendo reservado por otra solicitud, intenta de nuevo", ex);
            }
        }

        private static DateTime NormalizarFecha(DateTime fecha)
        {
            var local = fecha.Kind == DateTimeKind.Utc ? fecha.ToLocalTime() : fecha;
            return DateTime.SpecifyKind(local, DateTimeKind.Unspecified);
        }
    }
}
