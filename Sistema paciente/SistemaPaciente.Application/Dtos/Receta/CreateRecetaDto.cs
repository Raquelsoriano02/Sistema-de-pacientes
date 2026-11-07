using System.ComponentModel.DataAnnotations;

namespace SistemaPaciente.Application.Dtos.Receta
{
    public class CreateRecetaDto
    {
        [Range(1, int.MaxValue)]
        public int ConsultaId { get; set; }

        [Range(1, int.MaxValue)]
        public int MedicamentoId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Dosis { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Frecuencia { get; set; } = string.Empty;

        [Range(1, 365)]
        public int DuracionDias { get; set; }

        [MaxLength(500)]
        public string? Indicaciones { get; set; }
    }
}
