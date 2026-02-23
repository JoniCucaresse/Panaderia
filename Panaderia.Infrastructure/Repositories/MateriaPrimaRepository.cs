using Microsoft.EntityFrameworkCore;
using Panaderia.Domain.Entities;
using Panaderia.Domain.Repositories;
using Panaderia.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Infrastructure.Repositories
{
    public class MateriaPrimaRepository : IMateriaPrimaRepository
    {
        private readonly PanaderiaDbContext _context;

        public MateriaPrimaRepository(PanaderiaDbContext context)
        {
            _context = context;
        }

        public async Task<List<MateriaPrima>> GetAllAsync()
        {
            return await _context.MateriasPrimas
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }

        public async Task<MateriaPrima?> GetByIdAsync(int id)
        {
            return await _context.MateriasPrimas
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(MateriaPrima materiaPrima)
        {
            _context.MateriasPrimas.Add(materiaPrima);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MateriaPrima materiaPrima)
        {
            // ✅ Verificar que no sea null
            if (materiaPrima == null)
                throw new ArgumentNullException(nameof(materiaPrima));

            // ✅ Buscar la entidad existente en la BD
            var existing = await _context.MateriasPrimas.FindAsync(materiaPrima.Id);

            if (existing == null)
                throw new InvalidOperationException($"No se encontró la materia prima con Id {materiaPrima.Id}");

            // ✅ Actualizar los valores
            existing.Nombre = materiaPrima.Nombre;
            existing.UnidadMedida = materiaPrima.UnidadMedida;
            existing.CostoPorUnidad = materiaPrima.CostoPorUnidad;
            existing.FechaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var materia = await GetByIdAsync(id);
            if (materia == null)
                return;

            _context.MateriasPrimas.Remove(materia);
            await _context.SaveChangesAsync();
        }
    }
}
