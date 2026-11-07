using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SistemaPaciente.Domain.Core;

namespace SistemaPaciente.Application.Dtos.Horario
{
    public class CreateHorarioDto
    {
        [Range(1, int.MaxValue)]
        public int MedicoId { get; set; }

        [JsonRequired]
        public DiaSemana DiaSemana { get; set; }

        [JsonRequired]
        public TimeOnly HoraInicio { get; set; }

        [JsonRequired]
        public TimeOnly HoraFin { get; set; }

        public bool Disponible { get; set; } = true;
    }
}
