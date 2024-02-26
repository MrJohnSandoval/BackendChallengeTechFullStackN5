using Microsoft.EntityFrameworkCore;
using BackendChallengeTechFullStackN5.Models;

namespace BackendChallengeTechFullStackN5.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<TipoPermiso> TiposPermisos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Especificar el esquema para la tabla Permisos
            modelBuilder.Entity<Permiso>().ToTable("Permisos", schema: "n5");

            // Especificar el esquema para la tabla TipoPermisos
            modelBuilder.Entity<TipoPermiso>().ToTable("TipoPermisos", schema: "n5");
        }
    }
}
