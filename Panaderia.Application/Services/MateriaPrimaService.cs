using Microsoft.Extensions.Logging;
using Panaderia.App.Common;
using Panaderia.App.DTOs;
using Panaderia.App.Services.Interfaces;
using Panaderia.App.Validators;
using Panaderia.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.Services
{
    public class MateriaPrimaService : IMateriaPrimaService
    {
        private readonly IMateriaPrimaRepository _repository;
        private readonly ILogger<MateriaPrimaService>? _logger;

        public MateriaPrimaService(
            IMateriaPrimaRepository repository,
            ILogger<MateriaPrimaService>? logger = null)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<List<MateriaPrimaDto>>> ObtenerTodasAsync()
        {
            try
            {
                _logger?.LogInformation("Obteniendo todas las materias primas");

                var materias = await _repository.GetAllAsync();
                var dtos = materias.Select(MateriaPrimaDto.FromEntity).ToList();

                _logger?.LogInformation($"Se obtuvieron {dtos.Count} materias primas");

                return Result<List<MateriaPrimaDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al obtener materias primas");
                return Result<List<MateriaPrimaDto>>.Failure(
                    "Error al obtener las materias primas");
            }
        }

        public async Task<Result<MateriaPrimaDto>> ObtenerPorIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return Result<MateriaPrimaDto>.Failure("El ID debe ser mayor a 0");

                var materia = await _repository.GetByIdAsync(id);

                if (materia == null)
                    return Result<MateriaPrimaDto>.Failure(
                        $"No se encontró la materia prima con ID {id}");

                return Result<MateriaPrimaDto>.Success(
                    MateriaPrimaDto.FromEntity(materia));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error al obtener materia prima {id}");
                return Result<MateriaPrimaDto>.Failure(
                    "Error al obtener la materia prima");
            }
        }

        public async Task<Result<MateriaPrimaDto>> CrearAsync(CrearMateriaPrimaDto dto)
        {
            try
            {
                // Validar
                var errores = MateriaPrimaValidator.ValidarCreacion(dto);
                if (errores.Any())
                    return Result<MateriaPrimaDto>.Failure(errores);

                // Crear entidad
                var materia = dto.ToEntity();

                // Guardar
                await _repository.AddAsync(materia);

                _logger?.LogInformation($"Materia prima creada: {materia.Nombre}");

                return Result<MateriaPrimaDto>.Success(
                    MateriaPrimaDto.FromEntity(materia));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al crear materia prima");
                return Result<MateriaPrimaDto>.Failure(
                    "Error al crear la materia prima");
            }
        }

        public async Task<Result<MateriaPrimaDto>> ActualizarAsync(
            ActualizarMateriaPrimaDto dto)
        {
            try
            {
                // Validar
                var errores = MateriaPrimaValidator.ValidarActualizacion(dto);
                if (errores.Any())
                    return Result<MateriaPrimaDto>.Failure(errores);

                // Buscar existente
                var materiaExistente = await _repository.GetByIdAsync(dto.Id);
                if (materiaExistente == null)
                    return Result<MateriaPrimaDto>.Failure(
                        $"No se encontró la materia prima con ID {dto.Id}");

                // Actualizar
                materiaExistente.Nombre = dto.Nombre;
                materiaExistente.UnidadMedida = dto.UnidadMedida;
                materiaExistente.CostoPorUnidad = dto.CostoPorUnidad;
                materiaExistente.FechaActualizacion = DateTime.Now;

                await _repository.UpdateAsync(materiaExistente);

                _logger?.LogInformation($"Materia prima actualizada: {materiaExistente.Nombre}");

                return Result<MateriaPrimaDto>.Success(
                    MateriaPrimaDto.FromEntity(materiaExistente));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error al actualizar materia prima {dto.Id}");
                return Result<MateriaPrimaDto>.Failure(
                    "Error al actualizar la materia prima");
            }
        }

        public async Task<Result> EliminarAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return Result.Failure("El ID debe ser mayor a 0");

                var materia = await _repository.GetByIdAsync(id);
                if (materia == null)
                    return Result.Failure($"No se encontró la materia prima con ID {id}");

                await _repository.DeleteAsync(id);

                _logger?.LogInformation($"Materia prima eliminada: {materia.Nombre}");

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error al eliminar materia prima {id}");
                return Result.Failure("Error al eliminar la materia prima");
            }
        }

        public async Task<Result> ActivarDesactivarAsync(int id, bool activo)
        {
            try
            {
                var materia = await _repository.GetByIdAsync(id);
                if (materia == null)
                    return Result.Failure($"No se encontró la materia prima con ID {id}");

                materia.Activo = activo;
                await _repository.UpdateAsync(materia);

                _logger?.LogInformation(
                    $"Materia prima {(activo ? "activada" : "desactivada")}: {materia.Nombre}");

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error al cambiar estado de materia prima {id}");
                return Result.Failure("Error al cambiar el estado de la materia prima");
            }
        }

    }
}
