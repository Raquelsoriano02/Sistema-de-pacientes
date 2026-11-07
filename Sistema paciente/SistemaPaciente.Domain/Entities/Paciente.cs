using System.ComponentModel.DataAnnotations;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Domain.Entities
{
    public class Paciente : Person
    {
        [Required]
        [MaxLength(20)]
        public string Cedula { get; set; } = string.Empty;

        public DateOnly FechaNacimiento { get; set; }

        public Sexo Sexo { get; set; }

        public TipoSangre? TipoSangre { get; set; }

        [MaxLength(250)]
        public string? Direccion { get; set; }

        public int? SeguroId { get; set; }

        public virtual Seguro? Seguro { get; set; }

        [MaxLength(50)]
        public string? NumeroAfiliado { get; set; }

        public virtual ICollection<Alergia> Alergias { get; set; } = new List<Alergia>();

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
