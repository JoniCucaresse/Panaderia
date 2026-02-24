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
        public int Rinde { get; set; } = 1;
        public decimal CostoTotal { get; set; }
        public decimal? PrecioSugerido { get; set; }
        public bool Activo { get; set; } = true;

        // ✅ Navegación a recetas
        public List<Receta> Recetas { get; set; } = new();

        // ✅ Métodos de negocio
        public decimal CostoUnitario => Rinde > 0 ? CostoTotal / Rinde : 0;

        public void RecalcularCosto()
        {
            CostoTotal = Recetas?.Sum(r => r.CostoLinea) ?? 0;
        }

        public void CalcularPrecioSugerido(decimal margenPorcentaje)
        {
            if (margenPorcentaje <= 0)
                throw new ArgumentException("El margen debe ser mayor a 0");

            var costoUnitario = CostoUnitario;
            PrecioSugerido = costoUnitario * (1 + margenPorcentaje / 100);
        }
    }
}
