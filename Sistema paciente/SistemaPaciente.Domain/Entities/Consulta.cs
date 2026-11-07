using System.ComponentModel.DataAnnotations;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Domain.Entities
{
    public class Consulta : BaseEntity
    {
        public int CitaId { get; set; }

        public virtual Cita? Cita { get; set; }

        public decimal? PesoKg { get; set; }

        public decimal? TemperaturaC { get; set; }

        [MaxLength(20)]
        public string? PresionArterial { get; set; }

        [MaxLength(1000)]
        public string? Sintomas { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Diagnostico { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Tratamiento { get; set; }

        public virtual ICollection<Receta> Recetas { get; set; } = new List<Receta>();
    }
}
