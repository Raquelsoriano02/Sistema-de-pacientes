namespace SistemaPaciente.Application.Dtos.Seguro
{
    public class SeguroDto : DtoBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public decimal PorcentajeCobertura { get; set; }
        public bool Activo { get; set; }
    }
}
