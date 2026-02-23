using Panaderia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Domain.Repositories
{
    public interface IMateriaPrimaRepository
    {
        Task<List<MateriaPrima>> GetAllAsync();
        Task<MateriaPrima?> GetByIdAsync(int id);
        Task AddAsync(MateriaPrima materiaPrima);
        Task UpdateAsync(MateriaPrima materiaPrima);
        Task DeleteAsync(int id);
    }
}
