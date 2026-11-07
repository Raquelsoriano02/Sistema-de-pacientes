using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Consulta;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;

namespace SistemaPaciente.Application.Service
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _repository;

        public ConsultaService(IConsultaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<ConsultaDto>>> GetAllConsultasAsync()
        {
            var consultas = await _repository.GetAll();
            return ServiceResult<List<ConsultaDto>>.Ok(consultas.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<ConsultaDto>> GetConsultaByIdAsync(int id)
        {
            var consulta = await _repository.GetById(id);
            if (consulta == null)
            {
                return ServiceResult<ConsultaDto>.NotFoundResult($"Consulta con id {id} no encontrada");
            }

            return ServiceResult<ConsultaDto>.Ok(ToDto(consulta));
        }

        public async Task<ServiceResult<ConsultaDto>> CreateConsultaAsync(CreateConsultaDto dto)
        {
            try
            {
                var consulta = await _repository.CreateConsultaAsync(ToEntity(dto));
                return ServiceResult<ConsultaDto>.Ok(ToDto(consulta));
            }
            catch (ConsultaException ex)
            {
                return ServiceResult<ConsultaDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateConsultaAsync(int id, UpdateConsultaDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateConsultaAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Consulta con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (ConsultaException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteConsultaAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteConsultaAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Consulta con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (ConsultaException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static ConsultaDto ToDto(Consulta consulta)
        {
            return new ConsultaDto
            {
                Id = consulta.Id,
                FechaRegistro = consulta.FechaRegistro,
                CitaId = consulta.CitaId,
                PesoKg = consulta.PesoKg,
                TemperaturaC = consulta.TemperaturaC,
                PresionArterial = consulta.PresionArterial,
                Sintomas = consulta.Sintomas,
                Diagnostico = consulta.Diagnostico,
                Tratamiento = consulta.Tratamiento
            };
        }

        private static Consulta ToEntity(CreateConsultaDto dto)
        {
            return new Consulta
            {
                CitaId = dto.CitaId,
                PesoKg = dto.PesoKg,
                TemperaturaC = dto.TemperaturaC,
                PresionArterial = dto.PresionArterial,
                Sintomas = dto.Sintomas,
                Diagnostico = dto.Diagnostico,
                Tratamiento = dto.Tratamiento
            };
        }
    }
}
