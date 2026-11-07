using System.ComponentModel.DataAnnotations;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Domain.Entities
{
    public class Seguro : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Telefono { get; set; }

        public decimal PorcentajeCobertura { get; set; }

        public bool Activo { get; set; } = true;

        public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
    }
}
