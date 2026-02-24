using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.DTOs
{
    public class ActualizarProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Rinde { get; set; }
        public decimal? PrecioSugerido { get; set; }
        public List<ActualizarRecetaDto> Recetas { get; set; } = new();
    }

    public class ActualizarRecetaDto
    {
        public int? Id { get; set; } // Null si es nueva
        public int MateriaPrimaId { get; set; }
        public decimal Cantidad { get; set; }
    }
}
