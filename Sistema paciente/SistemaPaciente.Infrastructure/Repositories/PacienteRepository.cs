using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Core;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Repositories
{
    public class PacienteRepository : BaseRepository<Paciente>, IPacienteRepository
    {
        public PacienteRepository(SistemaPacienteContext context) : base(context)
        {
        }

        public async Task<Paciente> CreatePacienteAsync(Paciente datos)
        {
            await ValidarPacienteAsync(datos, 0);

            var paciente = new Paciente
            {
                Nombre = datos.Nombre,
                Apellido = datos.Apellido,
                Email = datos.Email,
                Telefono = datos.Telefono,
                Cedula = datos.Cedula,
                FechaNacimiento = datos.FechaNacimiento,
                Sexo = datos.Sexo,
                TipoSangre = datos.TipoSangre,
                Direccion = datos.Direccion,
                SeguroId = datos.SeguroId,
                NumeroAfiliado = datos.NumeroAfiliado
            };

            return await Create(paciente);
        }

        public async Task<bool> UpdatePacienteAsync(int id, Paciente datos)
        {
            var paciente = await GetById(id);
            if (paciente == null)
            {
                return false;
            }

            await ValidarPacienteAsync(datos, id);

            paciente.Nombre = datos.Nombre;
            paciente.Apellido = datos.Apellido;
            paciente.Email = datos.Email;
            paciente.Telefono = datos.Telefono;
            paciente.Cedula = datos.Cedula;
            paciente.FechaNacimiento = datos.FechaNacimiento;
            paciente.Sexo = datos.Sexo;
            paciente.TipoSangre = datos.TipoSangre;
            paciente.Direccion = datos.Direccion;
            paciente.SeguroId = datos.SeguroId;
            paciente.NumeroAfiliado = datos.NumeroAfiliado;

            await Update(paciente);
            return true;
        }

        public async Task<bool> DeletePacienteAsync(int id)
        {
            var paciente = await GetById(id);
            if (paciente == null)
            {
                return false;
            }

            var citas = await _context.Citas.CountAsync(c => c.PacienteId == id);
            if (citas > 0)
            {
                throw new PacienteException($"No se puede eliminar el paciente porque tiene {citas} cita(s) asociada(s)");
            }

            await Delete(id);
            return true;
        }

        private async Task ValidarPacienteAsync(Paciente datos, int id)
        {
            if (datos.FechaNacimiento > DateOnly.FromDateTime(DateTime.Today))
            {
                throw new PacienteException("La fecha de nacimiento no puede ser futura");
            }

            var cedulaExists = await _context.Pacientes.AnyAsync(p => p.Cedula == datos.Cedula && p.Id != id);
            if (cedulaExists)
            {
                throw new PacienteException($"La cédula {datos.Cedula} ya está registrada");
            }

            if (!string.IsNullOrEmpty(datos.Email))
            {
                var emailExists = await _context.Pacientes.AnyAsync(p => p.Email == datos.Email && p.Id != id);
                if (emailExists)
                {
                    throw new PacienteException($"El email {datos.Email} ya está registrado");
                }
            }

            if (datos.SeguroId != null)
            {
                var seguro = await _context.Seguros.FirstOrDefaultAsync(s => s.Id == datos.SeguroId);
                if (seguro == null)
                {
                    throw new PacienteException($"El seguro con ID {datos.SeguroId} no existe");
                }

                if (!seguro.Activo)
                {
                    throw new PacienteException($"El seguro {seguro.Nombre} no está activo");
                }
            }
        }
    }
}
