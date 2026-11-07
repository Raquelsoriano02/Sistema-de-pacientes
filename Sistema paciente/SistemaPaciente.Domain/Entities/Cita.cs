using System.ComponentModel.DataAnnotations;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Domain.Entities
{
    public class Cita : BaseEntity
    {
        public int PacienteId { get; set; }

        public virtual Paciente? Paciente { get; set; }

        public int MedicoId { get; set; }

        public virtual Medico? Medico { get; set; }

        public int ConsultorioId { get; set; }

        public virtual Consultorio? Consultorio { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraFin { get; set; }

        public decimal Costo { get; set; }

        public decimal Cobertura { get; set; }

        public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;

        [Required]
        [MaxLength(500)]
        public string Motivo { get; set; } = string.Empty;

        public virtual Consulta? Consulta { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
