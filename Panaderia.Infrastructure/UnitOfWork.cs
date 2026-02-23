using Panaderia.Domain;
using Panaderia.Domain.Repositories;
using Panaderia.Infrastructure.Persistence;
using Panaderia.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PanaderiaDbContext _context;
        private IMateriaPrimaRepository? _materiasPrimas;

        public UnitOfWork(PanaderiaDbContext context)
        {
            _context = context;
        }

        public IMateriaPrimaRepository MateriasPrimas =>
            _materiasPrimas ??= new MateriaPrimaRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
