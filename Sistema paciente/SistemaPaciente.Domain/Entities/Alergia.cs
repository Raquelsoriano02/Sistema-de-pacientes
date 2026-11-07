using System.ComponentModel.DataAnnotations;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Domain.Entities
{
    public class Alergia : BaseEntity
    {
        public int PacienteId { get; set; }

        public virtual Paciente? Paciente { get; set; }

        [Required]
        [MaxLength(150)]
        public string Sustancia { get; set; } = string.Empty;

        public Severidad Severidad { get; set; }

        [MaxLength(500)]
        public string? Observacion { get; set; }
    }
}
