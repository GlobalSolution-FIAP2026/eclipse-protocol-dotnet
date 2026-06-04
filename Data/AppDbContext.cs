using GlobalSolution.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Localizacao> Localizacoes { get; set; }
    public DbSet<Propriedade> Propriedades { get; set; }
    public DbSet<Plantacao> Plantacoes { get; set; }
    public DbSet<Sensor> Sensores { get; set; }
    public DbSet<Leitura> Leituras { get; set; }
    public DbSet<Alerta> Alertas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Propriedades)
            .WithOne(p => p.Usuario)
            .HasForeignKey(p => p.IdUsuario)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Localizacao>()
            .HasMany(l => l.Propriedades)
            .WithOne(p => p.Localizacao)
            .HasForeignKey(p => p.IdLocalizacao)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Propriedade>()
            .HasMany(p => p.Plantacoes)
            .WithOne(p => p.Propriedade)
            .HasForeignKey(p => p.IdPropriedade)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Plantacao>()
            .HasMany(p => p.Sensores)
            .WithOne(s => s.Plantacao)
            .HasForeignKey(s => s.IdPlantacao)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Sensor>()
            .HasMany(s => s.Leituras)
            .WithOne(l => l.Sensor)
            .HasForeignKey(l => l.IdSensor)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Plantacao>()
            .HasMany(p => p.Alertas)
            .WithOne(a => a.Plantacao)
            .HasForeignKey(a => a.IdPlantacao)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Leitura>()
            .HasMany(l => l.Alertas)
            .WithOne(a => a.Leitura)
            .HasForeignKey(a => a.IdLeitura)
            .OnDelete(DeleteBehavior.Restrict);
    }
}