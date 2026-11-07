using System.ComponentModel.DataAnnotations;

namespace SistemaPaciente.Application.Dtos.Medicamento
{
    public class CreateMedicamentoDto
    {
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string PrincipioActivo { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Presentacion { get; set; }

        public bool Activo { get; set; } = true;
    }
}
