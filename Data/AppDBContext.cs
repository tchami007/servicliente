using Microsoft.EntityFrameworkCore;
using ServiClientes.Model;

namespace ServiClientes.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);
                entity.Property(e => e.TipoDocumento).IsRequired().HasMaxLength(5);
                entity.Property(e => e.NumeroDocumento).IsRequired().HasMaxLength(11);
                entity.Property(e=>e.Apellidos).IsRequired().HasMaxLength(100);
                entity.Property(e=>e.Nombres).IsRequired().HasMaxLength(100);
                entity.Property(e=>e.Genero).IsRequired().HasMaxLength(1);
                entity.Property(e => e.FechaNacimiento).IsRequired();
            });
                
        }

    }
}
