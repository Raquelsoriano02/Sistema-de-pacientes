using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Especialidad;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;

namespace SistemaPaciente.Application.Service
{
    public class EspecialidadService : IEspecialidadService
    {
        private readonly IEspecialidadRepository _repository;

        public EspecialidadService(IEspecialidadRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<EspecialidadDto>>> GetAllEspecialidadesAsync()
        {
            var especialidades = await _repository.GetAll();
            return ServiceResult<List<EspecialidadDto>>.Ok(especialidades.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<EspecialidadDto>> GetEspecialidadByIdAsync(int id)
        {
            var especialidad = await _repository.GetById(id);
            if (especialidad == null)
            {
                return ServiceResult<EspecialidadDto>.NotFoundResult($"Especialidad con id {id} no encontrada");
            }

            return ServiceResult<EspecialidadDto>.Ok(ToDto(especialidad));
        }

        public async Task<ServiceResult<EspecialidadDto>> CreateEspecialidadAsync(CreateEspecialidadDto dto)
        {
            try
            {
                var especialidad = await _repository.CreateEspecialidadAsync(ToEntity(dto));
                return ServiceResult<EspecialidadDto>.Ok(ToDto(especialidad));
            }
            catch (EspecialidadException ex)
            {
                return ServiceResult<EspecialidadDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateEspecialidadAsync(int id, UpdateEspecialidadDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateEspecialidadAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Especialidad con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (EspecialidadException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteEspecialidadAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteEspecialidadAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Especialidad con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (EspecialidadException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static EspecialidadDto ToDto(Especialidad especialidad)
        {
            return new EspecialidadDto
            {
                Id = especialidad.Id,
                FechaRegistro = especialidad.FechaRegistro,
                Nombre = especialidad.Nombre,
                Descripcion = especialidad.Descripcion,
                Activa = especialidad.Activa
            };
        }

        private static Especialidad ToEntity(CreateEspecialidadDto dto)
        {
            return new Especialidad
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Activa = dto.Activa
            };
        }
    }
}
