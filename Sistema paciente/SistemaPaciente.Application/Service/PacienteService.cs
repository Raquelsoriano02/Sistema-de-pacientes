using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Paciente;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;

namespace SistemaPaciente.Application.Service
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<PacienteDto>>> GetAllPacientesAsync()
        {
            var pacientes = await _repository.GetAll();
            return ServiceResult<List<PacienteDto>>.Ok(pacientes.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<PacienteDto>> GetPacienteByIdAsync(int id)
        {
            var paciente = await _repository.GetById(id);
            if (paciente == null)
            {
                return ServiceResult<PacienteDto>.NotFoundResult($"Paciente con id {id} no encontrado");
            }

            return ServiceResult<PacienteDto>.Ok(ToDto(paciente));
        }

        public async Task<ServiceResult<PacienteDto>> CreatePacienteAsync(CreatePacienteDto dto)
        {
            try
            {
                var paciente = await _repository.CreatePacienteAsync(ToEntity(dto));
                return ServiceResult<PacienteDto>.Ok(ToDto(paciente));
            }
            catch (PacienteException ex)
            {
                return ServiceResult<PacienteDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdatePacienteAsync(int id, UpdatePacienteDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdatePacienteAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Paciente con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (PacienteException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeletePacienteAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeletePacienteAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Paciente con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (PacienteException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static PacienteDto ToDto(Paciente paciente)
        {
            return new PacienteDto
            {
                Id = paciente.Id,
                FechaRegistro = paciente.FechaRegistro,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                Email = paciente.Email,
                Telefono = paciente.Telefono,
                Cedula = paciente.Cedula,
                FechaNacimiento = paciente.FechaNacimiento,
                Sexo = paciente.Sexo,
                TipoSangre = paciente.TipoSangre,
                Direccion = paciente.Direccion,
                SeguroId = paciente.SeguroId,
                NumeroAfiliado = paciente.NumeroAfiliado
            };
        }

        private static Paciente ToEntity(CreatePacienteDto dto)
        {
            return new Paciente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Cedula = dto.Cedula,
                FechaNacimiento = dto.FechaNacimiento,
                Sexo = dto.Sexo,
                TipoSangre = dto.TipoSangre,
                Direccion = dto.Direccion,
                SeguroId = dto.SeguroId,
                NumeroAfiliado = dto.NumeroAfiliado
            };
        }
    }
}
