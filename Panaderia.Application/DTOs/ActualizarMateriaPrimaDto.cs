using Panaderia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.DTOs
{
    public class ActualizarMateriaPrimaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public UnidadMedida UnidadMedida { get; set; }
        public decimal CostoPorUnidad { get; set; }
    }
}
