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

        private string _nombre = string.Empty;
        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre es obligatorio");
                _nombre = value;
            }
        }

        public UnidadMedida UnidadMedida { get; set; }

        private decimal _costoPorUnidad;
        public decimal CostoPorUnidad
        {
            get => _costoPorUnidad;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El costo debe ser mayor a 0");
                _costoPorUnidad = value;
                FechaActualizacion = DateTime.Now;
            }
        }

        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;

        public void ActualizarCosto(decimal nuevoCosto)
        {
            CostoPorUnidad = nuevoCosto; // Usa el setter con validación
        }
    }
}
