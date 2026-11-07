using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Application.Dtos.Paciente
{
    public class CreatePacienteDto
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Cedula { get; set; } = string.Empty;

        [JsonRequired]
        public DateOnly FechaNacimiento { get; set; }

        [JsonRequired]
        public Sexo Sexo { get; set; }

        public TipoSangre? TipoSangre { get; set; }

        [MaxLength(250)]
        public string? Direccion { get; set; }

        [Range(1, int.MaxValue)]
        public int? SeguroId { get; set; }

        [MaxLength(50)]
        public string? NumeroAfiliado { get; set; }
    }
}
