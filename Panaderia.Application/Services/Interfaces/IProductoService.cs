using Panaderia.App.Common;
using Panaderia.App.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.Services.Interfaces
{
    public interface IProductoService
    {
        Task<Result<List<ProductoDto>>> ObtenerTodosAsync();
        Task<Result<ProductoConRecetasDto>> ObtenerPorIdAsync(int id);
        Task<Result<ProductoConRecetasDto>> CrearAsync(CrearProductoDto dto);
        Task<Result<ProductoConRecetasDto>> ActualizarAsync(ActualizarProductoDto dto);
        Task<Result> EliminarAsync(int id);
        Task<Result<ProductoConRecetasDto>> RecalcularCostosAsync(int id);
        Task<Result<ProductoConRecetasDto>> CalcularPrecioSugeridoAsync(int id, decimal margenPorcentaje);
    }
}
