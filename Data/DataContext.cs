using Microsoft.EntityFrameworkCore;
using presupuesto_api.Models;

namespace presupuesto_api.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Presupuesto> Presupuestos { get; set; }
    public DbSet<Proyeccion> Proyecciones { get; set; }

    //Para hacer que correo se unico en la base de datos
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(e => e.Correo)
                  .IsUnique();
        });

        modelBuilder.Entity<Proyeccion>()
        .Property(p => p.Monto)
        .HasColumnType("decimal(18,2)");
        
        modelBuilder.Entity<Proyeccion>()
        .HasOne(p => p.Categoria)
        .WithMany()
        .HasForeignKey(p => p.IdCategoria)
        .OnDelete(DeleteBehavior.SetNull);

    }

}