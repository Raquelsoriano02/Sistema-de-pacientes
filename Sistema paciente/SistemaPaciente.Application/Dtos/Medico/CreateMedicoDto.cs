using System.ComponentModel.DataAnnotations;

namespace SistemaPaciente.Application.Dtos.Medico
{
    public class CreateMedicoDto
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
        [MaxLength(30)]
        public string Licencia { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int EspecialidadId { get; set; }

        [Range(0, 9999999.99)]
        public decimal CostoConsulta { get; set; }

        public bool Activo { get; set; } = true;
    }
}
