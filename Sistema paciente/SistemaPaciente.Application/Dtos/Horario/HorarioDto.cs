using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Application.Dtos.Horario
{
    public class HorarioDto : DtoBase
    {
        public int MedicoId { get; set; }
        public DiaSemana DiaSemana { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public bool Disponible { get; set; }
    }
}
