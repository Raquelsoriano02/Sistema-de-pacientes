using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Medico;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;

namespace SistemaPaciente.Application.Service
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _repository;

        public MedicoService(IMedicoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<MedicoDto>>> GetAllMedicosAsync()
        {
            var medicos = await _repository.GetAll();
            return ServiceResult<List<MedicoDto>>.Ok(medicos.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<MedicoDto>> GetMedicoByIdAsync(int id)
        {
            var medico = await _repository.GetById(id);
            if (medico == null)
            {
                return ServiceResult<MedicoDto>.NotFoundResult($"Médico con id {id} no encontrado");
            }

            return ServiceResult<MedicoDto>.Ok(ToDto(medico));
        }

        public async Task<ServiceResult<MedicoDto>> CreateMedicoAsync(CreateMedicoDto dto)
        {
            try
            {
                var medico = await _repository.CreateMedicoAsync(ToEntity(dto));
                return ServiceResult<MedicoDto>.Ok(ToDto(medico));
            }
            catch (MedicoException ex)
            {
                return ServiceResult<MedicoDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateMedicoAsync(int id, UpdateMedicoDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateMedicoAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Médico con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (MedicoException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteMedicoAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteMedicoAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Médico con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (MedicoException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static MedicoDto ToDto(Medico medico)
        {
            return new MedicoDto
            {
                Id = medico.Id,
                FechaRegistro = medico.FechaRegistro,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Email = medico.Email,
                Telefono = medico.Telefono,
                Licencia = medico.Licencia,
                EspecialidadId = medico.EspecialidadId,
                CostoConsulta = medico.CostoConsulta,
                Activo = medico.Activo
            };
        }

        private static Medico ToEntity(CreateMedicoDto dto)
        {
            return new Medico
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Licencia = dto.Licencia,
                EspecialidadId = dto.EspecialidadId,
                CostoConsulta = dto.CostoConsulta,
                Activo = dto.Activo
            };
        }
    }
}
