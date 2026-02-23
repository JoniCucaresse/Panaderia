using Panaderia.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IMateriaPrimaRepository MateriasPrimas { get; }
        Task<int> SaveChangesAsync();
    }
}
