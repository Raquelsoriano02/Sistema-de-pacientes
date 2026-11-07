using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Medicamento;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;

namespace SistemaPaciente.Application.Service
{
    public class MedicamentoService : IMedicamentoService
    {
        private readonly IMedicamentoRepository _repository;

        public MedicamentoService(IMedicamentoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<MedicamentoDto>>> GetAllMedicamentosAsync()
        {
            var medicamentos = await _repository.GetAll();
            return ServiceResult<List<MedicamentoDto>>.Ok(medicamentos.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<MedicamentoDto>> GetMedicamentoByIdAsync(int id)
        {
            var medicamento = await _repository.GetById(id);
            if (medicamento == null)
            {
                return ServiceResult<MedicamentoDto>.NotFoundResult($"Medicamento con id {id} no encontrado");
            }

            return ServiceResult<MedicamentoDto>.Ok(ToDto(medicamento));
        }

        public async Task<ServiceResult<MedicamentoDto>> CreateMedicamentoAsync(CreateMedicamentoDto dto)
        {
            try
            {
                var medicamento = await _repository.CreateMedicamentoAsync(ToEntity(dto));
                return ServiceResult<MedicamentoDto>.Ok(ToDto(medicamento));
            }
            catch (MedicamentoException ex)
            {
                return ServiceResult<MedicamentoDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateMedicamentoAsync(int id, UpdateMedicamentoDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateMedicamentoAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Medicamento con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (MedicamentoException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteMedicamentoAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteMedicamentoAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Medicamento con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (MedicamentoException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static MedicamentoDto ToDto(Medicamento medicamento)
        {
            return new MedicamentoDto
            {
                Id = medicamento.Id,
                FechaRegistro = medicamento.FechaRegistro,
                Nombre = medicamento.Nombre,
                PrincipioActivo = medicamento.PrincipioActivo,
                Presentacion = medicamento.Presentacion,
                Activo = medicamento.Activo
            };
        }

        private static Medicamento ToEntity(CreateMedicamentoDto dto)
        {
            return new Medicamento
            {
                Nombre = dto.Nombre,
                PrincipioActivo = dto.PrincipioActivo,
                Presentacion = dto.Presentacion,
                Activo = dto.Activo
            };
        }
    }
}
