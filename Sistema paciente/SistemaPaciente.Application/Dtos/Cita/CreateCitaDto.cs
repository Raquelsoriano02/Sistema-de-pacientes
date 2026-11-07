using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemaPaciente.Application.Dtos.Cita
{
    public class CreateCitaDto
    {
        [Range(1, int.MaxValue)]
        public int PacienteId { get; set; }

        [Range(1, int.MaxValue)]
        public int MedicoId { get; set; }

        [Range(1, int.MaxValue)]
        public int ConsultorioId { get; set; }

        [JsonRequired]
        public DateTime FechaHoraInicio { get; set; }

        [Required]
        [MaxLength(500)]
        public string Motivo { get; set; } = string.Empty;
    }
}
