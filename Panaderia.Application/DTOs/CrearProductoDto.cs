using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.DTOs
{
    public class CrearProductoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int Rinde { get; set; } = 1;
        public decimal? PrecioSugerido { get; set; }
        public List<CrearRecetaDto> Recetas { get; set; } = new();
    }
    public class CrearRecetaDto
    {
        public int MateriaPrimaId { get; set; }
        public decimal Cantidad { get; set; }
    }
}
