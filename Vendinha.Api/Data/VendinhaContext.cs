using Microsoft.EntityFrameworkCore;
using Vendinha.Api.Models;

namespace Vendinha.Api.Data;

public class VendinhaContext : DbContext
{
    public VendinhaContext(DbContextOptions<VendinhaContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Divida> Dividas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Cpf)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(150);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(11);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Email)
            .HasMaxLength(150);

        modelBuilder.Entity<Divida>()
            .HasOne(d => d.Cliente)
            .WithMany(c => c.Dividas)
            .HasForeignKey(d => d.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
