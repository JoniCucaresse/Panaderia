using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Panaderia.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Infrastructure.Factories
{
    public class PanaderiaDbContextFactory : IDesignTimeDbContextFactory<PanaderiaDbContext>
    {
        public PanaderiaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PanaderiaDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=.\\SQLEXPRESS02;Database=PanaderiaDb;Trusted_Connection=True;TrustServerCertificate=True");

            return new PanaderiaDbContext(optionsBuilder.Options);
        }
    }
}
