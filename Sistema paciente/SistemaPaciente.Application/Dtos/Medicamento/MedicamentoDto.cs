namespace SistemaPaciente.Application.Dtos.Medicamento
{
    public class MedicamentoDto : DtoBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string PrincipioActivo { get; set; } = string.Empty;
        public string? Presentacion { get; set; }
        public bool Activo { get; set; }
    }
}
