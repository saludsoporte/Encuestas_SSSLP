using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EncuestasProxyApi.Models;

public partial class ProxyDbContext : DbContext
{
    public ProxyDbContext()
    {
    }

    public ProxyDbContext(DbContextOptions<ProxyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dispositivo> Dispositivos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Encuesta_SSSLP;Username=postgres;Password=ivan");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Dispositivo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dispositivos_pkey");

            entity.ToTable("dispositivos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(false)
                .HasColumnName("activo");
            entity.Property(e => e.DeviceId)
                .HasMaxLength(100)
                .HasColumnName("device_id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
