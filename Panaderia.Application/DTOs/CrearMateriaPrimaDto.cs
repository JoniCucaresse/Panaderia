using Panaderia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.DTOs
{
    public class CrearMateriaPrimaDto
    {
        public string Nombre { get; set; } = string.Empty;
        public UnidadMedida UnidadMedida { get; set; }
        public decimal CostoPorUnidad { get; set; }

        public MateriaPrima ToEntity()
        {
            return new MateriaPrima
            {
                Nombre = Nombre,
                UnidadMedida = UnidadMedida,
                CostoPorUnidad = CostoPorUnidad,
                FechaActualizacion = DateTime.Now,
                Activo = true
            };
        }
    }
}
