using System.ComponentModel.DataAnnotations;

namespace SistemaPaciente.Application.Dtos.Seguro
{
    public class CreateSeguroDto
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Telefono { get; set; }

        [Range(0, 100)]
        public decimal PorcentajeCobertura { get; set; }

        public bool Activo { get; set; } = true;
    }
}
