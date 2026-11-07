using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Pago;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Domain.Entities;
using SistemaPaciente.Infrastructure.Exceptions;
using SistemaPaciente.Infrastructure.Interfaces;

namespace SistemaPaciente.Application.Service
{
    public class PagoService : IPagoService
    {
        private readonly IPagoRepository _repository;

        public PagoService(IPagoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<List<PagoDto>>> GetAllPagosAsync()
        {
            var pagos = await _repository.GetAll();
            return ServiceResult<List<PagoDto>>.Ok(pagos.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<PagoDto>> GetPagoByIdAsync(int id)
        {
            var pago = await _repository.GetById(id);
            if (pago == null)
            {
                return ServiceResult<PagoDto>.NotFoundResult($"Pago con id {id} no encontrado");
            }

            return ServiceResult<PagoDto>.Ok(ToDto(pago));
        }

        public async Task<ServiceResult<PagoDto>> CreatePagoAsync(CreatePagoDto dto)
        {
            try
            {
                var pago = await _repository.CreatePagoAsync(ToEntity(dto));
                return ServiceResult<PagoDto>.Ok(ToDto(pago));
            }
            catch (PagoException ex)
            {
                return ServiceResult<PagoDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdatePagoAsync(int id, UpdatePagoDto dto)
        {
            try
            {
                var actualizado = await _repository.UpdatePagoAsync(id, ToEntity(dto));
                if (!actualizado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Pago con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (PagoException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeletePagoAsync(int id)
        {
            try
            {
                var eliminado = await _repository.DeletePagoAsync(id);
                if (!eliminado)
                {
                    return ServiceResult<bool>.NotFoundResult($"Pago con id {id} no encontrado");
                }

                return ServiceResult<bool>.Ok(true);
            }
            catch (PagoException ex)
            {
                return ServiceResult<bool>.Fail(ex.Message);
            }
        }

        private static PagoDto ToDto(Pago pago)
        {
            return new PagoDto
            {
                Id = pago.Id,
                FechaRegistro = pago.FechaRegistro,
                CitaId = pago.CitaId,
                Monto = pago.Monto,
                MetodoPago = pago.MetodoPago,
                FechaPago = pago.FechaPago,
                Referencia = pago.Referencia
            };
        }

        private static Pago ToEntity(CreatePagoDto dto)
        {
            return new Pago
            {
                CitaId = dto.CitaId,
                Monto = dto.Monto,
                MetodoPago = dto.MetodoPago,
                Referencia = dto.Referencia
            };
        }
    }
}
