using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Application.Dtos.Alergia
{
    public class AlergiaDto : DtoBase
    {
        public int PacienteId { get; set; }
        public string Sustancia { get; set; } = string.Empty;
        public Severidad Severidad { get; set; }
        public string? Observacion { get; set; }
    }
}
