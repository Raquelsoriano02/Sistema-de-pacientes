using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Seguro;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;

namespace SistemaPaciente.Application.Service
{
    public class SeguroService : ISeguroService
    {
        private readonly ISeguroRepository _repository;

        public SeguroService(ISeguroRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<SeguroDto>>> GetAllSegurosAsync()
        {
            var seguros = await _repository.GetAll();
            return ServiceResult<List<SeguroDto>>.Ok(seguros.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<SeguroDto>> GetSeguroByIdAsync(int id)
        {
            var seguro = await _repository.GetById(id);
            if (seguro == null)
            {
                return ServiceResult<SeguroDto>.NotFoundResult($"Seguro con id {id} no encontrado");
            }

            return ServiceResult<SeguroDto>.Ok(ToDto(seguro));
        }

        public async Task<ServiceResult<SeguroDto>> CreateSeguroAsync(CreateSeguroDto dto)
        {
            try
            {
                var seguro = await _repository.CreateSeguroAsync(ToEntity(dto));
                return ServiceResult<SeguroDto>.Ok(ToDto(seguro));
            }
            catch (SeguroException ex)
            {
                return ServiceResult<SeguroDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateSeguroAsync(int id, UpdateSeguroDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateSeguroAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Seguro con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (SeguroException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteSeguroAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteSeguroAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Seguro con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (SeguroException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static SeguroDto ToDto(Seguro seguro)
        {
            return new SeguroDto
            {
                Id = seguro.Id,
                FechaRegistro = seguro.FechaRegistro,
                Nombre = seguro.Nombre,
                Telefono = seguro.Telefono,
                PorcentajeCobertura = seguro.PorcentajeCobertura,
                Activo = seguro.Activo
            };
        }

        private static Seguro ToEntity(CreateSeguroDto dto)
        {
            return new Seguro
            {
                Nombre = dto.Nombre,
                Telefono = dto.Telefono,
                PorcentajeCobertura = dto.PorcentajeCobertura,
                Activo = dto.Activo
            };
        }
    }
}
