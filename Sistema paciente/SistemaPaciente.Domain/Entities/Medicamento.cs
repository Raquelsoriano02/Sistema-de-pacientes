using System.ComponentModel.DataAnnotations;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Domain.Entities
{
    public class Medicamento : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string PrincipioActivo { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Presentacion { get; set; }

        public bool Activo { get; set; } = true;

        public virtual ICollection<Receta> Recetas { get; set; } = new List<Receta>();
    }
}
