
using Microsoft.EntityFrameworkCore;
using SistemaPrestamosLibros.Domain.Entities;
using System.Reflection.Emit;

namespace SistemaPrestamosLibros.Infrastructure.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Libro> Libros { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Devolucion> Devoluciones { get; set; }

 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Devolucion>()
                .Property(d => d.MultaMora)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Libro)
                .WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.LibroId);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Estudiante)
                .WithMany(e => e.Prestamos)
                .HasForeignKey(p => p.EstudianteId);

            modelBuilder.Entity<Devolucion>()
                .HasOne(d => d.Prestamo)
                .WithOne(p => p.Devolucion)
                .HasForeignKey<Devolucion>(d => d.PrestamoId);
        }
    }
}
