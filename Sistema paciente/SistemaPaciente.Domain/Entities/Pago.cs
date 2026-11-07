using System.ComponentModel.DataAnnotations;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Domain.Entities
{
    public class Pago : BaseEntity
    {
        public int CitaId { get; set; }

        public virtual Cita? Cita { get; set; }

        public decimal Monto { get; set; }

        public MetodoPago MetodoPago { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.Now;

        [MaxLength(100)]
        public string? Referencia { get; set; }
    }
}
