using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Alergia;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;

namespace SistemaPaciente.Application.Service
{
    public class AlergiaService : IAlergiaService
    {
        private readonly IAlergiaRepository _repository;

        public AlergiaService(IAlergiaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<AlergiaDto>>> GetAllAlergiasAsync()
        {
            var alergias = await _repository.GetAll();
            return ServiceResult<List<AlergiaDto>>.Ok(alergias.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<AlergiaDto>> GetAlergiaByIdAsync(int id)
        {
            var alergia = await _repository.GetById(id);
            if (alergia == null)
            {
                return ServiceResult<AlergiaDto>.NotFoundResult($"Alergia con id {id} no encontrada");
            }

            return ServiceResult<AlergiaDto>.Ok(ToDto(alergia));
        }

        public async Task<ServiceResult<AlergiaDto>> CreateAlergiaAsync(CreateAlergiaDto dto)
        {
            try
            {
                var alergia = await _repository.CreateAlergiaAsync(ToEntity(dto));
                return ServiceResult<AlergiaDto>.Ok(ToDto(alergia));
            }
            catch (AlergiaException ex)
            {
                return ServiceResult<AlergiaDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateAlergiaAsync(int id, UpdateAlergiaDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateAlergiaAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Alergia con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (AlergiaException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteAlergiaAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteAlergiaAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Alergia con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (AlergiaException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static AlergiaDto ToDto(Alergia alergia)
        {
            return new AlergiaDto
            {
                Id = alergia.Id,
                FechaRegistro = alergia.FechaRegistro,
                PacienteId = alergia.PacienteId,
                Sustancia = alergia.Sustancia,
                Severidad = alergia.Severidad,
                Observacion = alergia.Observacion
            };
        }

        private static Alergia ToEntity(CreateAlergiaDto dto)
        {
            return new Alergia
            {
                PacienteId = dto.PacienteId,
                Sustancia = dto.Sustancia,
                Severidad = dto.Severidad,
                Observacion = dto.Observacion
            };
        }
    }
}
