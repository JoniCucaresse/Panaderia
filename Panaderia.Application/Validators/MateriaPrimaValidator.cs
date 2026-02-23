using Panaderia.App.DTOs;
using Panaderia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.Validators
{
    public class MateriaPrimaValidator
    {
        public static List<string> ValidarCreacion(CrearMateriaPrimaDto dto)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                errores.Add("El nombre es obligatorio");
            else if (dto.Nombre.Length > 100)
                errores.Add("El nombre no puede superar los 100 caracteres");

            if (dto.CostoPorUnidad <= 0)
                errores.Add("El costo debe ser mayor a 0");

            if (!Enum.IsDefined(typeof(UnidadMedida), dto.UnidadMedida))
                errores.Add("La unidad de medida no es válida");

            return errores;
        }

        public static List<string> ValidarActualizacion(ActualizarMateriaPrimaDto dto)
        {
            var errores = new List<string>();

            if (dto.Id <= 0)
                errores.Add("El ID debe ser mayor a 0");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                errores.Add("El nombre es obligatorio");
            else if (dto.Nombre.Length > 100)
                errores.Add("El nombre no puede superar los 100 caracteres");

            if (dto.CostoPorUnidad <= 0)
                errores.Add("El costo debe ser mayor a 0");

            if (!Enum.IsDefined(typeof(UnidadMedida), dto.UnidadMedida))
                errores.Add("La unidad de medida no es válida");

            return errores;
        }
    }
}
