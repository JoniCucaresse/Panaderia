using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Domain.Entities
{
    public class Receta
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
        public int MateriaPrimaId { get; set; }
        public MateriaPrima MateriaPrima { get; set; } = null!;
        public decimal Cantidad { get; set; }

        // ✅ Propiedad calculada
        public decimal CostoLinea => MateriaPrima?.CostoPorUnidad * Cantidad ?? 0;
    }
}
