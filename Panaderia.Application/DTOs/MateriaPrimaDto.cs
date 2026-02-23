using Panaderia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.DTOs
{
    public class MateriaPrimaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public UnidadMedida UnidadMedida { get; set; }
        public decimal CostoPorUnidad { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool Activo { get; set; }

        // Mapeo desde Entidad
        public static MateriaPrimaDto FromEntity(MateriaPrima entity)
        {
            return new MateriaPrimaDto
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                UnidadMedida = entity.UnidadMedida,
                CostoPorUnidad = entity.CostoPorUnidad,
                FechaActualizacion = entity.FechaActualizacion,
                Activo = entity.Activo
            };
        }

        // Mapeo a Entidad
        public MateriaPrima ToEntity()
        {
            return new MateriaPrima
            {
                Id = Id,
                Nombre = Nombre,
                UnidadMedida = UnidadMedida,
                CostoPorUnidad = CostoPorUnidad,
                FechaActualizacion = FechaActualizacion,
                Activo = Activo
            };
        }
    }
}
