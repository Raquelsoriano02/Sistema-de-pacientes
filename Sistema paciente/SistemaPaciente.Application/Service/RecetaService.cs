using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Receta;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;

namespace SistemaPaciente.Application.Service
{
    public class RecetaService : IRecetaService
    {
        private readonly IRecetaRepository _repository;

        public RecetaService(IRecetaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<RecetaDto>>> GetAllRecetasAsync()
        {
            var recetas = await _repository.GetAll();
            return ServiceResult<List<RecetaDto>>.Ok(recetas.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<RecetaDto>> GetRecetaByIdAsync(int id)
        {
            var receta = await _repository.GetById(id);
            if (receta == null)
            {
                return ServiceResult<RecetaDto>.NotFoundResult($"Receta con id {id} no encontrada");
            }

            return ServiceResult<RecetaDto>.Ok(ToDto(receta));
        }

        public async Task<ServiceResult<RecetaDto>> CreateRecetaAsync(CreateRecetaDto dto)
        {
            try
            {
                var receta = await _repository.CreateRecetaAsync(ToEntity(dto));
                return ServiceResult<RecetaDto>.Ok(ToDto(receta));
            }
            catch (RecetaException ex)
            {
                return ServiceResult<RecetaDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateRecetaAsync(int id, UpdateRecetaDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateRecetaAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Receta con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (RecetaException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteRecetaAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteRecetaAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Receta con id {id} no encontrada");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (RecetaException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static RecetaDto ToDto(Receta receta)
        {
            return new RecetaDto
            {
                Id = receta.Id,
                FechaRegistro = receta.FechaRegistro,
                ConsultaId = receta.ConsultaId,
                MedicamentoId = receta.MedicamentoId,
                Dosis = receta.Dosis,
                Frecuencia = receta.Frecuencia,
                DuracionDias = receta.DuracionDias,
                Indicaciones = receta.Indicaciones
            };
        }

        private static Receta ToEntity(CreateRecetaDto dto)
        {
            return new Receta
            {
                ConsultaId = dto.ConsultaId,
                MedicamentoId = dto.MedicamentoId,
                Dosis = dto.Dosis,
                Frecuencia = dto.Frecuencia,
                DuracionDias = dto.DuracionDias,
                Indicaciones = dto.Indicaciones
            };
        }
    }
}
