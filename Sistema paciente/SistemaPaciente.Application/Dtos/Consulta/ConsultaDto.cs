namespace SistemaPaciente.Application.Dtos.Consulta
{
    public class ConsultaDto : DtoBase
    {
        public int CitaId { get; set; }
        public decimal? PesoKg { get; set; }
        public decimal? TemperaturaC { get; set; }
        public string? PresionArterial { get; set; }
        public string? Sintomas { get; set; }
        public string Diagnostico { get; set; } = string.Empty;
        public string? Tratamiento { get; set; }
    }
}
