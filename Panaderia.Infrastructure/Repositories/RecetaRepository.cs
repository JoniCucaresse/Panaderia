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
    public class RecetaRepository : IRecetaRepository
    {
        private readonly PanaderiaDbContext _context;

        public RecetaRepository(PanaderiaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Receta>> GetByProductoIdAsync(int productoId)
        {
            return await _context.Recetas
                .Include(r => r.MateriaPrima)
                .Where(r => r.ProductoId == productoId)
                .ToListAsync();
        }

        public async Task AddAsync(Receta receta)
        {
            _context.Recetas.Add(receta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Receta receta)
        {
            _context.Recetas.Update(receta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var receta = await _context.Recetas.FindAsync(id);
            if (receta == null)
                return;

            _context.Recetas.Remove(receta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByProductoIdAsync(int productoId)
        {
            var recetas = await _context.Recetas
                .Where(r => r.ProductoId == productoId)
                .ToListAsync();

            _context.Recetas.RemoveRange(recetas);
            await _context.SaveChangesAsync();
        }
    }
}
