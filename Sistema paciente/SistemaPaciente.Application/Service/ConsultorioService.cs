using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Consultorio;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;

namespace SistemaPaciente.Application.Service
{
    public class ConsultorioService : IConsultorioService
    {
        private readonly IConsultorioRepository _repository;

        public ConsultorioService(IConsultorioRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<ConsultorioDto>>> GetAllConsultoriosAsync()
        {
            var consultorios = await _repository.GetAll();
            return ServiceResult<List<ConsultorioDto>>.Ok(consultorios.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<ConsultorioDto>> GetConsultorioByIdAsync(int id)
        {
            var consultorio = await _repository.GetById(id);
            if (consultorio == null)
            {
                return ServiceResult<ConsultorioDto>.NotFoundResult($"Consultorio con id {id} no encontrado");
            }

            return ServiceResult<ConsultorioDto>.Ok(ToDto(consultorio));
        }

        public async Task<ServiceResult<ConsultorioDto>> CreateConsultorioAsync(CreateConsultorioDto dto)
        {
            try
            {
                var consultorio = await _repository.CreateConsultorioAsync(ToEntity(dto));
                return ServiceResult<ConsultorioDto>.Ok(ToDto(consultorio));
            }
            catch (ConsultorioException ex)
            {
                return ServiceResult<ConsultorioDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateConsultorioAsync(int id, UpdateConsultorioDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateConsultorioAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Consultorio con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (ConsultorioException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteConsultorioAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteConsultorioAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Consultorio con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (ConsultorioException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static ConsultorioDto ToDto(Consultorio consultorio)
        {
            return new ConsultorioDto
            {
                Id = consultorio.Id,
                FechaRegistro = consultorio.FechaRegistro,
                Codigo = consultorio.Codigo,
                Ubicacion = consultorio.Ubicacion,
                Activo = consultorio.Activo
            };
        }

        private static Consultorio ToEntity(CreateConsultorioDto dto)
        {
            return new Consultorio
            {
                Codigo = dto.Codigo,
                Ubicacion = dto.Ubicacion,
                Activo = dto.Activo
            };
        }
    }
}
