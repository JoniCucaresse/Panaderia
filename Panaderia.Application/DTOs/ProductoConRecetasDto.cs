using Panaderia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.DTOs
{
    public class ProductoConRecetasDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Rinde { get; set; }
        public decimal CostoTotal { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal? PrecioSugerido { get; set; }
        public bool Activo { get; set; }
        public List<RecetaDto> Recetas { get; set; } = new();

        public static ProductoConRecetasDto FromEntity(Producto entity)
        {
            return new ProductoConRecetasDto
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Rinde = entity.Rinde,
                CostoTotal = entity.CostoTotal,
                CostoUnitario = entity.CostoUnitario,
                PrecioSugerido = entity.PrecioSugerido,
                Activo = entity.Activo,
                Recetas = entity.Recetas?.Select(RecetaDto.FromEntity).ToList() ?? new()
            };
        }
    }
}
