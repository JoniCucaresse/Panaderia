using Microsoft.EntityFrameworkCore;
using Panaderia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Infrastructure.Persistence
{
    public class PanaderiaDbContext : DbContext
    {
        public PanaderiaDbContext(DbContextOptions<PanaderiaDbContext> options)
        : base(options)
        {
        }

        public DbSet<MateriaPrima> MateriasPrimas => Set<MateriaPrima>();
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Receta> Recetas => Set<Receta>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MateriaPrima>()
                .Property(x => x.CostoPorUnidad)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Producto>()
                .Property(x => x.CostoTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Receta>()
                .Property(x => x.Cantidad)
                .HasPrecision(18, 3);
        }
    }
}
