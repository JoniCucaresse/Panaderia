using Panaderia.App.Common;
using Panaderia.App.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.Services.Interfaces
{
    public interface IMateriaPrimaService
    {
        Task<Result<List<MateriaPrimaDto>>> ObtenerTodasAsync();
        Task<Result<MateriaPrimaDto>> ObtenerPorIdAsync(int id);
        Task<Result<MateriaPrimaDto>> CrearAsync(CrearMateriaPrimaDto dto);
        Task<Result<MateriaPrimaDto>> ActualizarAsync(ActualizarMateriaPrimaDto dto);
        Task<Result> EliminarAsync(int id);
        Task<Result> ActivarDesactivarAsync(int id, bool activo);
    }
}
