using System.ComponentModel.DataAnnotations;

namespace SistemaPaciente.Application.Dtos.Consulta
{
    public class CreateConsultaDto
    {
        [Range(1, int.MaxValue)]
        public int CitaId { get; set; }

        [Range(0.1, 500)]
        public decimal? PesoKg { get; set; }

        [Range(25, 45)]
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
    }
}
