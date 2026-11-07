using System.ComponentModel.DataAnnotations;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Domain.Entities
{
    public class Consultorio : BaseEntity
    {
        [Required]
        [MaxLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Ubicacion { get; set; }

        public bool Activo { get; set; } = true;

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
