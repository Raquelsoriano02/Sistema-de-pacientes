namespace SistemaPaciente.Application.Dtos.Especialidad
{
    public class EspecialidadDto : DtoBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Activa { get; set; }
    }
}
