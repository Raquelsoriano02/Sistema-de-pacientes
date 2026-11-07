using System.ComponentModel.DataAnnotations;

namespace SistemaPaciente.Application.Dtos.Consultorio
{
    public class CreateConsultorioDto
    {
        [Required]
        [MaxLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Ubicacion { get; set; }

        public bool Activo { get; set; } = true;
    }
}
