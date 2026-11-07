using System.ComponentModel.DataAnnotations;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Domain.Entities
{
    public class Receta : BaseEntity
    {
        public int ConsultaId { get; set; }

        public virtual Consulta? Consulta { get; set; }

        public int MedicamentoId { get; set; }

        public virtual Medicamento? Medicamento { get; set; }

        [Required]
        [MaxLength(100)]
        public string Dosis { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Frecuencia { get; set; } = string.Empty;

        public int DuracionDias { get; set; }

        [MaxLength(500)]
        public string? Indicaciones { get; set; }
    }
}
