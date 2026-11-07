using System.ComponentModel.DataAnnotations;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Domain.Entities
{
    public class Medico : Person
    {
        [Required]
        [MaxLength(30)]
        public string Licencia { get; set; } = string.Empty;

        public int EspecialidadId { get; set; }

        public virtual Especialidad? Especialidad { get; set; }

        public decimal CostoConsulta { get; set; }

        public bool Activo { get; set; } = true;

        public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
