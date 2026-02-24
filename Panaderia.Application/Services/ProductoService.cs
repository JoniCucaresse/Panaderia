using Panaderia.App.Common;
using Panaderia.App.DTOs;
using Panaderia.App.Services.Interfaces;
using Panaderia.App.Validators;
using Panaderia.Domain.Entities;
using Panaderia.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IRecetaRepository _recetaRepository;
        private readonly IMateriaPrimaRepository _materiaPrimaRepository;

        public ProductoService(
            IProductoRepository productoRepository,
            IRecetaRepository recetaRepository,
            IMateriaPrimaRepository materiaPrimaRepository)
        {
            _productoRepository = productoRepository;
            _recetaRepository = recetaRepository;
            _materiaPrimaRepository = materiaPrimaRepository;
        }

        public async Task<Result<List<ProductoDto>>> ObtenerTodosAsync()
        {
            try
            {
                var productos = await _productoRepository.GetAllAsync();
                var dtos = productos.Select(ProductoDto.FromEntity).ToList();
                return Result<List<ProductoDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return Result<List<ProductoDto>>.Failure("Error al obtener los productos");
            }
        }

        public async Task<Result<ProductoConRecetasDto>> ObtenerPorIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return Result<ProductoConRecetasDto>.Failure("El ID debe ser mayor a 0");

                var producto = await _productoRepository.GetByIdWithRecetasAsync(id);

                if (producto == null)
                    return Result<ProductoConRecetasDto>.Failure(
                        $"No se encontró el producto con ID {id}");

                return Result<ProductoConRecetasDto>.Success(
                    ProductoConRecetasDto.FromEntity(producto));
            }
            catch (Exception ex)
            {
                return Result<ProductoConRecetasDto>.Failure("Error al obtener el producto");
            }
        }

        public async Task<Result<ProductoConRecetasDto>> CrearAsync(CrearProductoDto dto)
        {
            try
            {
                // Validar
                var errores = ProductoValidator.ValidarCreacion(dto);
                if (errores.Any())
                    return Result<ProductoConRecetasDto>.Failure(errores);

                // Verificar nombre duplicado
                if (await _productoRepository.ExisteNombreAsync(dto.Nombre))
                    return Result<ProductoConRecetasDto>.Failure(
                        $"Ya existe un producto con el nombre '{dto.Nombre}'");

                // Verificar que todas las materias primas existan
                foreach (var recetaDto in dto.Recetas)
                {
                    var materia = await _materiaPrimaRepository.GetByIdAsync(recetaDto.MateriaPrimaId);
                    if (materia == null)
                        return Result<ProductoConRecetasDto>.Failure(
                            $"No se encontró la materia prima con ID {recetaDto.MateriaPrimaId}");
                }

                // Crear producto
                var producto = new Producto
                {
                    Nombre = dto.Nombre,
                    Rinde = dto.Rinde,
                    PrecioSugerido = dto.PrecioSugerido,
                    Activo = true
                };

                // Crear recetas
                foreach (var recetaDto in dto.Recetas)
                {
                    var materia = await _materiaPrimaRepository.GetByIdAsync(recetaDto.MateriaPrimaId);

                    producto.Recetas.Add(new Receta
                    {
                        MateriaPrimaId = recetaDto.MateriaPrimaId,
                        MateriaPrima = materia!,
                        Cantidad = recetaDto.Cantidad
                    });
                }

                // Calcular costo
                producto.RecalcularCosto();

                // Guardar
                await _productoRepository.AddAsync(producto);

                // Recargar con relaciones
                var productoCreado = await _productoRepository.GetByIdWithRecetasAsync(producto.Id);

                return Result<ProductoConRecetasDto>.Success(
                    ProductoConRecetasDto.FromEntity(productoCreado!));
            }
            catch (Exception ex)
            {
                return Result<ProductoConRecetasDto>.Failure("Error al crear el producto");
            }
        }

        public async Task<Result<ProductoConRecetasDto>> ActualizarAsync(ActualizarProductoDto dto)
        {
            try
            {
                // Validar
                var errores = ProductoValidator.ValidarActualizacion(dto);
                if (errores.Any())
                    return Result<ProductoConRecetasDto>.Failure(errores);

                // Buscar existente
                var producto = await _productoRepository.GetByIdWithRecetasAsync(dto.Id);
                if (producto == null)
                    return Result<ProductoConRecetasDto>.Failure(
                        $"No se encontró el producto con ID {dto.Id}");

                // Verificar nombre duplicado
                if (await _productoRepository.ExisteNombreAsync(dto.Nombre, dto.Id))
                    return Result<ProductoConRecetasDto>.Failure(
                        $"Ya existe otro producto con el nombre '{dto.Nombre}'");

                // Verificar que todas las materias primas existan
                foreach (var recetaDto in dto.Recetas)
                {
                    var materia = await _materiaPrimaRepository.GetByIdAsync(recetaDto.MateriaPrimaId);
                    if (materia == null)
                        return Result<ProductoConRecetasDto>.Failure(
                            $"No se encontró la materia prima con ID {recetaDto.MateriaPrimaId}");
                }

                // Actualizar producto
                producto.Nombre = dto.Nombre;
                producto.Rinde = dto.Rinde;
                producto.PrecioSugerido = dto.PrecioSugerido;

                // Eliminar recetas antiguas
                await _recetaRepository.DeleteByProductoIdAsync(producto.Id);

                // Agregar nuevas recetas
                producto.Recetas.Clear();
                foreach (var recetaDto in dto.Recetas)
                {
                    var materia = await _materiaPrimaRepository.GetByIdAsync(recetaDto.MateriaPrimaId);

                    var receta = new Receta
                    {
                        ProductoId = producto.Id,
                        MateriaPrimaId = recetaDto.MateriaPrimaId,
                        MateriaPrima = materia!,
                        Cantidad = recetaDto.Cantidad
                    };

                    producto.Recetas.Add(receta);
                    await _recetaRepository.AddAsync(receta);
                }

                // Recalcular costo
                producto.RecalcularCosto();

                // Guardar
                await _productoRepository.UpdateAsync(producto);

                // Recargar con relaciones
                var productoActualizado = await _productoRepository.GetByIdWithRecetasAsync(producto.Id);

                return Result<ProductoConRecetasDto>.Success(
                    ProductoConRecetasDto.FromEntity(productoActualizado!));
            }
            catch (Exception ex)
            {
                return Result<ProductoConRecetasDto>.Failure("Error al actualizar el producto");
            }
        }

        public async Task<Result> EliminarAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return Result.Failure("El ID debe ser mayor a 0");

                var producto = await _productoRepository.GetByIdAsync(id);
                if (producto == null)
                    return Result.Failure($"No se encontró el producto con ID {id}");

                // Primero eliminar recetas
                await _recetaRepository.DeleteByProductoIdAsync(id);

                // Luego eliminar producto
                await _productoRepository.DeleteAsync(id);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure("Error al eliminar el producto");
            }
        }

        public async Task<Result<ProductoConRecetasDto>> RecalcularCostosAsync(int id)
        {
            try
            {
                var producto = await _productoRepository.GetByIdWithRecetasAsync(id);
                if (producto == null)
                    return Result<ProductoConRecetasDto>.Failure(
                        $"No se encontró el producto con ID {id}");

                producto.RecalcularCosto();
                await _productoRepository.UpdateAsync(producto);

                var productoActualizado = await _productoRepository.GetByIdWithRecetasAsync(id);

                return Result<ProductoConRecetasDto>.Success(
                    ProductoConRecetasDto.FromEntity(productoActualizado!));
            }
            catch (Exception ex)
            {
                return Result<ProductoConRecetasDto>.Failure("Error al recalcular los costos");
            }
        }

        public async Task<Result<ProductoConRecetasDto>> CalcularPrecioSugeridoAsync(
            int id, decimal margenPorcentaje)
        {
            try
            {
                if (margenPorcentaje <= 0)
                    return Result<ProductoConRecetasDto>.Failure(
                        "El margen debe ser mayor a 0");

                var producto = await _productoRepository.GetByIdWithRecetasAsync(id);
                if (producto == null)
                    return Result<ProductoConRecetasDto>.Failure(
                        $"No se encontró el producto con ID {id}");

                producto.CalcularPrecioSugerido(margenPorcentaje);
                await _productoRepository.UpdateAsync(producto);

                var productoActualizado = await _productoRepository.GetByIdWithRecetasAsync(id);

                return Result<ProductoConRecetasDto>.Success(
                    ProductoConRecetasDto.FromEntity(productoActualizado!));
            }
            catch (Exception ex)
            {
                return Result<ProductoConRecetasDto>.Failure(
                    "Error al calcular el precio sugerido");
            }
        }
    }
}
