using Panaderia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Domain.Repositories
{
    public interface IRecetaRepository
    {
        Task<List<Receta>> GetByProductoIdAsync(int productoId);
        Task AddAsync(Receta receta);
        Task UpdateAsync(Receta receta);
        Task DeleteAsync(int id);
        Task DeleteByProductoIdAsync(int productoId);
    }
}
