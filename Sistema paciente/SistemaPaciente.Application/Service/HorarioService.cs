using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Horario;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;

namespace SistemaPaciente.Application.Service
{
    public class HorarioService : IHorarioService
    {
        private readonly IHorarioRepository _repository;

        public HorarioService(IHorarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<HorarioDto>>> GetAllHorariosAsync()
        {
            var horarios = await _repository.GetAll();
            return ServiceResult<List<HorarioDto>>.Ok(horarios.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<HorarioDto>> GetHorarioByIdAsync(int id)
        {
            var horario = await _repository.GetById(id);
            if (horario == null)
            {
                return ServiceResult<HorarioDto>.NotFoundResult($"Horario con id {id} no encontrado");
            }

            return ServiceResult<HorarioDto>.Ok(ToDto(horario));
        }

        public async Task<ServiceResult<HorarioDto>> CreateHorarioAsync(CreateHorarioDto dto)
        {
            try
            {
                var horario = await _repository.CreateHorarioAsync(ToEntity(dto));
                return ServiceResult<HorarioDto>.Ok(ToDto(horario));
            }
            catch (HorarioException ex)
            {
                return ServiceResult<HorarioDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateHorarioAsync(int id, UpdateHorarioDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdateHorarioAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Horario con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (HorarioException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteHorarioAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteHorarioAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Horario con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (HorarioException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static HorarioDto ToDto(Horario horario)
        {
            return new HorarioDto
            {
                Id = horario.Id,
                FechaRegistro = horario.FechaRegistro,
                MedicoId = horario.MedicoId,
                DiaSemana = horario.DiaSemana,
                HoraInicio = horario.HoraInicio,
                HoraFin = horario.HoraFin,
                Disponible = horario.Disponible
            };
        }

        private static Horario ToEntity(CreateHorarioDto dto)
        {
            return new Horario
            {
                MedicoId = dto.MedicoId,
                DiaSemana = dto.DiaSemana,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                Disponible = dto.Disponible
            };
        }
    }
}
