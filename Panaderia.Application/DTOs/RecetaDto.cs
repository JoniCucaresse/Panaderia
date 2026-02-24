using Panaderia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.DTOs
{
    public class RecetaDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int MateriaPrimaId { get; set; }
        public string MateriaPrimaNombre { get; set; } = string.Empty;
        public UnidadMedida UnidadMedida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CostoPorUnidad { get; set; }
        public decimal CostoLinea { get; set; }

        public static RecetaDto FromEntity(Receta entity)
        {
            return new RecetaDto
            {
                Id = entity.Id,
                ProductoId = entity.ProductoId,
                MateriaPrimaId = entity.MateriaPrimaId,
                MateriaPrimaNombre = entity.MateriaPrima?.Nombre ?? string.Empty,
                UnidadMedida = entity.MateriaPrima?.UnidadMedida ?? UnidadMedida.Gramo,
                Cantidad = entity.Cantidad,
                CostoPorUnidad = entity.MateriaPrima?.CostoPorUnidad ?? 0,
                CostoLinea = entity.CostoLinea
            };
        }
    }
}
