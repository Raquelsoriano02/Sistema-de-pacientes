using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Application.Dtos.Paciente
{
    public class PacienteDto : DtoBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public DateOnly FechaNacimiento { get; set; }
        public Sexo Sexo { get; set; }
        public TipoSangre? TipoSangre { get; set; }
        public string? Direccion { get; set; }
        public int? SeguroId { get; set; }
        public string? NumeroAfiliado { get; set; }
    }
}
