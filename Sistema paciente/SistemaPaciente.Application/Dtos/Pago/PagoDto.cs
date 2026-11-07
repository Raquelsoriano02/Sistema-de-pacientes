using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Application.Dtos.Pago
{
    public class PagoDto : DtoBase
    {
        public int CitaId { get; set; }
        public decimal Monto { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public DateTime FechaPago { get; set; }
        public string? Referencia { get; set; }
    }
}
