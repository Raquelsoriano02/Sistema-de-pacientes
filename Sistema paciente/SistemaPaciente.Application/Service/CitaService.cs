using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Cita;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Core;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;

namespace SistemaPaciente.Application.Service
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _repository;

        public CitaService(ICitaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<CitaDto>>> GetAllCitasAsync()
        {
            var citas = await _repository.GetAll();
            return ServiceResult<List<CitaDto>>.Ok(citas.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<CitaDto>> GetCitaByIdAsync(int id)
        {
            var cita = await _repository.GetById(id);
            if (cita == null)
            {
                return ServiceResult<CitaDto>.NotFoundResult($"Cita con id {id} no encontrada");
            }

            return ServiceResult<CitaDto>.Ok(ToDto(cita));
        }

        public async Task<ServiceResult<List<DateTime>>> GetDisponibilidadAsync(int medicoId, int consultorioId, DateOnly fecha)
        {
            try
            {
                var disponibles = await _repository.GetDisponibilidadAsync(medicoId, consultorioId, fecha);
                return ServiceResult<List<DateTime>>.Ok(disponibles);
            }
            catch (CitaException ex)
            {
                return ServiceResult<List<DateTime>>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateEstadoAsync(int id, EstadoCita estado)
        {
            try
            {
                var actualizado = await _repository.UpdateEstadoAsync(id, estado);
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Cita con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (CitaException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<CitaDto>> CreateCitaAsync(CreateCitaDto dto)
        {
            try
            {
                var cita = await _repository.CreateCitaAsync(ToEntity(dto));
                return ServiceResult<CitaDto>.Ok(ToDto(cita));
            }
            catch (CitaException ex)
            {
                return ServiceResult<CitaDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateCitaAsync(int id, UpdateCitaDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateCitaAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Cita con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (CitaException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteCitaAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteCitaAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Cita con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (CitaException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static CitaDto ToDto(Cita cita)
        {
            return new CitaDto
            {
                Id = cita.Id,
                FechaRegistro = cita.FechaRegistro,
                PacienteId = cita.PacienteId,
                MedicoId = cita.MedicoId,
                ConsultorioId = cita.ConsultorioId,
                FechaHoraInicio = cita.FechaHoraInicio,
                FechaHoraFin = cita.FechaHoraFin,
                Costo = cita.Costo,
                Cobertura = cita.Cobertura,
                Estado = cita.Estado,
                Motivo = cita.Motivo
            };
        }

        private static Cita ToEntity(CreateCitaDto dto)
        {
            return new Cita
            {
                PacienteId = dto.PacienteId,
                MedicoId = dto.MedicoId,
                ConsultorioId = dto.ConsultorioId,
                FechaHoraInicio = dto.FechaHoraInicio,
                Motivo = dto.Motivo
            };
        }
    }
}
