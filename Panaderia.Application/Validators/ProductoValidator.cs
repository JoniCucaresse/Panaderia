using Panaderia.App.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.Validators
{
    public class ProductoValidator
    {
        public static List<string> ValidarCreacion(CrearProductoDto dto)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                errores.Add("El nombre es obligatorio");
            else if (dto.Nombre.Length > 100)
                errores.Add("El nombre no puede superar los 100 caracteres");

            if (dto.Rinde <= 0)
                errores.Add("El rinde debe ser mayor a 0");

            if (dto.PrecioSugerido.HasValue && dto.PrecioSugerido.Value < 0)
                errores.Add("El precio sugerido no puede ser negativo");

            if (dto.Recetas == null || !dto.Recetas.Any())
                errores.Add("El producto debe tener al menos una materia prima en la receta");

            // Validar recetas
            foreach (var receta in dto.Recetas ?? new())
            {
                if (receta.MateriaPrimaId <= 0)
                    errores.Add("ID de materia prima inválido en la receta");

                if (receta.Cantidad <= 0)
                    errores.Add("La cantidad debe ser mayor a 0 en todas las recetas");
            }

            // Validar duplicados en recetas
            var duplicados = dto.Recetas?
                .GroupBy(r => r.MateriaPrimaId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            if (duplicados != null && duplicados.Any())
                errores.Add("No puede haber materias primas duplicadas en la receta");

            return errores;
        }

        public static List<string> ValidarActualizacion(ActualizarProductoDto dto)
        {
            var errores = new List<string>();

            if (dto.Id <= 0)
                errores.Add("El ID debe ser mayor a 0");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                errores.Add("El nombre es obligatorio");
            else if (dto.Nombre.Length > 100)
                errores.Add("El nombre no puede superar los 100 caracteres");

            if (dto.Rinde <= 0)
                errores.Add("El rinde debe ser mayor a 0");

            if (dto.PrecioSugerido.HasValue && dto.PrecioSugerido.Value < 0)
                errores.Add("El precio sugerido no puede ser negativo");

            if (dto.Recetas == null || !dto.Recetas.Any())
                errores.Add("El producto debe tener al menos una materia prima en la receta");

            // Validar recetas
            foreach (var receta in dto.Recetas ?? new())
            {
                if (receta.MateriaPrimaId <= 0)
                    errores.Add("ID de materia prima inválido en la receta");

                if (receta.Cantidad <= 0)
                    errores.Add("La cantidad debe ser mayor a 0 en todas las recetas");
            }

            // Validar duplicados
            var duplicados = dto.Recetas?
                .GroupBy(r => r.MateriaPrimaId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            if (duplicados != null && duplicados.Any())
                errores.Add("No puede haber materias primas duplicadas en la receta");

            return errores;
        }
    }
}
