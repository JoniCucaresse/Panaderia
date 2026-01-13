using Microsoft.EntityFrameworkCore;
using Panaderia.Domain.Entities;
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
            _context.MateriasPrimas.Update(materiaPrima);
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
