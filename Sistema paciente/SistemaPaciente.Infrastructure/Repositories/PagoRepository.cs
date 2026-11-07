using SistemaPaciente.Domain.Core;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Core;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Repositories
{
    public class PagoRepository : BaseRepository<Pago>, IPagoRepository
    {
        public PagoRepository(SistemaPacienteContext context) : base(context)
        {
        }

        public async Task<Pago> CreatePagoAsync(Pago datos)
        {
            await ValidarPagoAsync(datos, 0);

            var pago = new Pago
            {
                CitaId = datos.CitaId,
                Monto = datos.Monto,
                MetodoPago = datos.MetodoPago,
                FechaPago = DateTime.Now,
                Referencia = datos.Referencia
            };

            return await Create(pago);
        }

        public async Task<bool> UpdatePagoAsync(int id, Pago datos)
        {
            var pago = await GetById(id);
            if (pago == null)
            {
                return false;
            }

            await ValidarPagoAsync(datos, id);

            pago.CitaId = datos.CitaId;
            pago.Monto = datos.Monto;
            pago.MetodoPago = datos.MetodoPago;
            pago.Referencia = datos.Referencia;

            await Update(pago);
            return true;
        }

        public async Task<bool> DeletePagoAsync(int id)
        {
            var pago = await Delete(id);
            return pago != null;
        }

        private async Task ValidarPagoAsync(Pago datos, int id)
        {
            var cita = await _context.Citas.FirstOrDefaultAsync(c => c.Id == datos.CitaId);
            if (cita == null)
            {
                throw new PagoException($"La cita con ID {datos.CitaId} no existe");
            }

            if (cita.Estado == EstadoCita.Cancelada)
            {
                throw new PagoException("No se puede registrar un pago en una cita cancelada");
            }

            var pagado = await _context.Pagos
                .Where(p => p.CitaId == datos.CitaId && p.Id != id)
                .SumAsync(p => p.Monto);

            var total = cita.Costo - cita.Cobertura;
            if (pagado + datos.Monto > total)
            {
                throw new PagoException($"El pago excede el saldo pendiente de {total - pagado}");
            }
        }
    }
}
