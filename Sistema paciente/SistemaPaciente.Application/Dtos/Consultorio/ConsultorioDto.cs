namespace SistemaPaciente.Application.Dtos.Consultorio
{
    public class ConsultorioDto : DtoBase
    {
        public string Codigo { get; set; } = string.Empty;
        public string? Ubicacion { get; set; }
        public bool Activo { get; set; }
    }
}
