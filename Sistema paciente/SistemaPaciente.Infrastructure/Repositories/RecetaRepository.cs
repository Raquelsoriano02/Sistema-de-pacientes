using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Core;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Repositories
{
    public class RecetaRepository : BaseRepository<Receta>, IRecetaRepository
    {
        public RecetaRepository(SistemaPacienteContext context) : base(context)
        {
        }

        public async Task<Receta> CreateRecetaAsync(Receta datos)
        {
            await ValidarRecetaAsync(datos, 0);

            var receta = new Receta
            {
                ConsultaId = datos.ConsultaId,
                MedicamentoId = datos.MedicamentoId,
                Dosis = datos.Dosis,
                Frecuencia = datos.Frecuencia,
                DuracionDias = datos.DuracionDias,
                Indicaciones = datos.Indicaciones
            };

            return await Create(receta);
        }

        public async Task<bool> UpdateRecetaAsync(int id, Receta datos)
        {
            var receta = await GetById(id);
            if (receta == null)
            {
                return false;
            }

            await ValidarRecetaAsync(datos, id);

            receta.ConsultaId = datos.ConsultaId;
            receta.MedicamentoId = datos.MedicamentoId;
            receta.Dosis = datos.Dosis;
            receta.Frecuencia = datos.Frecuencia;
            receta.DuracionDias = datos.DuracionDias;
            receta.Indicaciones = datos.Indicaciones;

            await Update(receta);
            return true;
        }

        public async Task<bool> DeleteRecetaAsync(int id)
        {
            var receta = await Delete(id);
            return receta != null;
        }

        private async Task ValidarRecetaAsync(Receta datos, int id)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Cita)
                .FirstOrDefaultAsync(c => c.Id == datos.ConsultaId);

            if (consulta == null)
            {
                throw new RecetaException($"La consulta con ID {datos.ConsultaId} no existe");
            }

            var medicamento = await _context.Medicamentos.FirstOrDefaultAsync(m => m.Id == datos.MedicamentoId);
            if (medicamento == null)
            {
                throw new RecetaException($"El medicamento con ID {datos.MedicamentoId} no existe");
            }

            if (!medicamento.Activo)
            {
                throw new RecetaException($"El medicamento {medicamento.Nombre} no está activo");
            }

            var duplicada = await _context.Recetas.AnyAsync(r =>
                r.ConsultaId == datos.ConsultaId &&
                r.MedicamentoId == datos.MedicamentoId &&
                r.Id != id);

            if (duplicada)
            {
                throw new RecetaException($"La consulta ya tiene una receta de {medicamento.Nombre}");
            }

            var alergico = await _context.Alergias.AnyAsync(a =>
                a.PacienteId == consulta.Cita!.PacienteId &&
                (a.Sustancia == medicamento.Nombre || a.Sustancia == medicamento.PrincipioActivo));

            if (alergico)
            {
                throw new RecetaException($"El paciente tiene registrada una alergia a {medicamento.Nombre}");
            }
        }
    }
}
