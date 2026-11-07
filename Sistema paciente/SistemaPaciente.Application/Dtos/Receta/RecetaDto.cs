namespace SistemaPaciente.Application.Dtos.Receta
{
    public class RecetaDto : DtoBase
    {
        public int ConsultaId { get; set; }
        public int MedicamentoId { get; set; }
        public string Dosis { get; set; } = string.Empty;
        public string Frecuencia { get; set; } = string.Empty;
        public int DuracionDias { get; set; }
        public string? Indicaciones { get; set; }
    }
}
