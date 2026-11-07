using System.ComponentModel.DataAnnotations;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Domain.Entities
{
    public class Especialidad : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        public bool Activa { get; set; } = true;

        public virtual ICollection<Medico> Medicos { get; set; } = new List<Medico>();
    }
}
