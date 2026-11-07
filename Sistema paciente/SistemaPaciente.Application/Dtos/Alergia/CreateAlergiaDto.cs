using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Application.Dtos.Alergia
{
    public class CreateAlergiaDto
    {
        [Range(1, int.MaxValue)]
        public int PacienteId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Sustancia { get; set; } = string.Empty;

        [JsonRequired]
        public Severidad Severidad { get; set; }

        [MaxLength(500)]
        public string? Observacion { get; set; }
    }
}
