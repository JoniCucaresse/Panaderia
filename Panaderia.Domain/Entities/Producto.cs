using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Domain.Entities
{
    public class Producto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        // Cantidad de unidades que rinde la receta
        // Ej: 1 pan, 12 facturas, 20 panes
        public int Rinde { get; set; } = 1;

        public decimal CostoTotal { get; set; }

        public decimal? PrecioSugerido { get; set; }

        public bool Activo { get; set; } = true;
    }
}
