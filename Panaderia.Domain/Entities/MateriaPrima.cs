using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Domain.Entities
{
    public class MateriaPrima
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        // Ej: Gramo, Mililitro, Unidad
        public UnidadMedida UnidadMedida { get; set; }

        // Costo por unidad base (ej: costo por gramo)
        public decimal CostoPorUnidad { get; set; }

        public DateTime FechaActualizacion { get; set; } = DateTime.Now;

        public bool Activo { get; set; } = true;

        public void ActualizarCosto(decimal nuevoCosto)
        {
            if (nuevoCosto <= 0)
                throw new ArgumentException("El costo debe ser mayor a 0");

            CostoPorUnidad = nuevoCosto;
            FechaActualizacion = DateTime.Now;
        }
    }
}
