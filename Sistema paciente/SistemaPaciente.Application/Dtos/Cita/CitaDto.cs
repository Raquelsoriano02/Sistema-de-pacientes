using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Application.Dtos.Cita
{
    public class CitaDto : DtoBase
    {
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public int ConsultorioId { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public decimal Costo { get; set; }
        public decimal Cobertura { get; set; }
        public EstadoCita Estado { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
