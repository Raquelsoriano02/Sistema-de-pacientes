using SistemaPaciente.Application.Core;
using SistemaPaciente.Application.Dtos.Horario;

namespace SistemaPaciente.Application.Interfaces
{
    public interface IHorarioService
    {
        Task<ServiceResult<List<HorarioDto>>> GetAllHorariosAsync();
        Task<ServiceResult<HorarioDto>> GetHorarioByIdAsync(int id);
        Task<ServiceResult<HorarioDto>> CreateHorarioAsync(CreateHorarioDto dto);
        Task<ServiceResult<bool>> UpdateHorarioAsync(int id, UpdateHorarioDto dto);
        Task<ServiceResult<bool>> DeleteHorarioAsync(int id);
    }
}
