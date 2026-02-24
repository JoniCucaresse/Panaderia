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
    public class ProductoRepository : IProductoRepository
    {
        private readonly PanaderiaDbContext _context;

        public ProductoRepository(PanaderiaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> GetAllAsync()
        {
            return await _context.Productos
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Producto?> GetByIdWithRecetasAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.Recetas)
                    .ThenInclude(r => r.MateriaPrima)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Producto producto)
        {
            var existing = await _context.Productos.FindAsync(producto.Id);

            if (existing == null)
                throw new InvalidOperationException($"No se encontró el producto con Id {producto.Id}");

            existing.Nombre = producto.Nombre;
            existing.Rinde = producto.Rinde;
            existing.CostoTotal = producto.CostoTotal;
            existing.PrecioSugerido = producto.PrecioSugerido;
            existing.Activo = producto.Activo;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await GetByIdAsync(id);
            if (producto == null)
                return;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteNombreAsync(string nombre, int? excludeId = null)
        {
            return await _context.Productos
                .AnyAsync(p => p.Nombre.ToLower() == nombre.ToLower()
                    && (!excludeId.HasValue || p.Id != excludeId.Value));
        }

    }
}
