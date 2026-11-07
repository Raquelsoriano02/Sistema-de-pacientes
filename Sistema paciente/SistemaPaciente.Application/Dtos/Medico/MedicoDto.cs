namespace SistemaPaciente.Application.Dtos.Medico
{
    public class MedicoDto : DtoBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Licencia { get; set; } = string.Empty;
        public int EspecialidadId { get; set; }
        public decimal CostoConsulta { get; set; }
        public bool Activo { get; set; }
    }
}
