using System.ComponentModel.DataAnnotations;

namespace SistemaPaciente.Application.Dtos.Especialidad
{
    public class CreateEspecialidadDto
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        public bool Activa { get; set; } = true;
    }
}
