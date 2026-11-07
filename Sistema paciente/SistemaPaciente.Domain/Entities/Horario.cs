using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Domain.Entities
{
    public class Horario : BaseEntity
    {
        public int MedicoId { get; set; }

        public virtual Medico? Medico { get; set; }

        public DiaSemana DiaSemana { get; set; }

        public TimeOnly HoraInicio { get; set; }

        public TimeOnly HoraFin { get; set; }

        public bool Disponible { get; set; } = true;
    }
}
