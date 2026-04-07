using Microsoft.EntityFrameworkCore;

namespace EncuestasApi.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<DispositivoAutorizado> DispositivosAutorizados { get; set; }
    public virtual DbSet<VersionEncuesta> VersionesEncuesta { get; set; }
    public virtual DbSet<Encuesta> Encuestas { get; set; }
    public virtual DbSet<PreguntaVersion> PreguntasVersion { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuarios_pkey");

            entity.ToTable("usuarios", "app");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.NombreUsuario).HasMaxLength(100).HasColumnName("usuario");
            entity.Property(e => e.Password).HasMaxLength(100).HasColumnName("password");
            entity.Property(e => e.Brigada).HasMaxLength(100).HasColumnName("brigada");
            entity.Property(e => e.Jurisdiccion).HasMaxLength(100).HasColumnName("jurisdiccion");
            entity.Property(e => e.Rol).HasMaxLength(100).HasColumnName("rol");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");
        });

        modelBuilder.Entity<DispositivoAutorizado>(entity =>
        {
            entity.HasKey(e => e.IdDispositivo).HasName("dispositivos_autorizados_pkey");

            entity.ToTable("dispositivos_autorizados", "app");

            entity.Property(e => e.IdDispositivo).HasColumnName("id_dispositivo");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Identificador).HasMaxLength(100).HasColumnName("identificador");
            entity.Property(e => e.SerialDispositivo).HasMaxLength(100).HasColumnName("serial_dispositivo");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");

            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.DispositivosAutorizados)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("fk_usuario");
        });

        modelBuilder.Entity<VersionEncuesta>(entity =>
        {
            entity.HasKey(e => e.IdVersion).HasName("version_encuesta_pkey");

            entity.ToTable("version_encuesta", "catalogos");

            entity.Property(e => e.IdVersion).HasColumnName("id_version");
            entity.Property(e => e.Descripcion).HasMaxLength(255).HasColumnName("descripcion");
            entity.Property(e => e.Activa).HasColumnName("activa");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
        });

        modelBuilder.Entity<Encuesta>(entity =>
        {
            entity.HasKey(e => e.IdEncuesta).HasName("encuestas_pkey");

            entity.ToTable("encuestas", "app");

            entity.Property(e => e.IdEncuesta).HasColumnName("id_encuesta");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdDispositivo).HasColumnName("id_dispositivo");
            entity.Property(e => e.IdVersion).HasColumnName("id_version");
            entity.Property(e => e.Curp).HasMaxLength(18).HasColumnName("curp");
            entity.Property(e => e.FechaCaptura).HasColumnName("fecha_captura");
            entity.Property(e => e.FechaSincronizacion).HasColumnName("fecha_sincronizacion");
            entity.Property(e => e.Latitud).HasMaxLength(100).HasColumnName("latitud");
            entity.Property(e => e.Longitud).HasMaxLength(100).HasColumnName("longitud");
            entity.Property(e => e.PuertaAbierta).HasColumnName("puertaabierta");
            entity.Property(e => e.ObsPuerta).HasMaxLength(255).HasColumnName("obspuerta");
            entity.Property(e => e.Participa).HasColumnName("participa");
            entity.Property(e => e.ObsParticipa).HasMaxLength(255).HasColumnName("obsparticipa");

            entity.Property(e => e.Seccion1).HasColumnType("jsonb").HasColumnName("seccion1");
            entity.Property(e => e.Seccion2).HasColumnType("jsonb").HasColumnName("seccion2");
            entity.Property(e => e.Seccion3).HasColumnType("jsonb").HasColumnName("seccion3");
            entity.Property(e => e.Seccion4).HasColumnType("jsonb").HasColumnName("seccion4");
            entity.Property(e => e.Seccion5).HasColumnType("jsonb").HasColumnName("seccion5");
            entity.Property(e => e.Seccion6).HasColumnType("jsonb").HasColumnName("seccion6");
            entity.Property(e => e.Seccion7).HasColumnType("jsonb").HasColumnName("seccion7");
            entity.Property(e => e.Seccion8).HasColumnType("jsonb").HasColumnName("seccion8");
            entity.Property(e => e.Seccion9).HasColumnType("jsonb").HasColumnName("seccion9");
            entity.Property(e => e.Seccion10).HasColumnType("jsonb").HasColumnName("seccion10");
            entity.Property(e => e.Seccion11).HasColumnType("jsonb").HasColumnName("seccion11");
            entity.Property(e => e.Seccion12).HasColumnType("jsonb").HasColumnName("seccion12");
            entity.Property(e => e.Seccion13).HasColumnType("jsonb").HasColumnName("seccion13");

            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.Encuestas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("fk_usuario");

            entity.HasOne(d => d.Dispositivo)
                .WithMany(p => p.Encuestas)
                .HasForeignKey(d => d.IdDispositivo)
                .HasConstraintName("fk_dispositivo");

            entity.HasOne(d => d.Version)
                .WithMany(p => p.Encuestas)
                .HasForeignKey(d => d.IdVersion)
                .HasConstraintName("fk_version");
        });

        modelBuilder.Entity<PreguntaVersion>(entity =>
        {
            entity.HasKey(e => e.IdPregunta).HasName("preguntas_version_pkey");

            entity.ToTable("preguntas_version", "catalogos");

            entity.Property(e => e.IdPregunta).HasColumnName("id_pregunta");
            entity.Property(e => e.IdVersion).HasColumnName("id_version");
            entity.Property(e => e.Pregunta).HasMaxLength(100).HasColumnName("pregunta");
            entity.Property(e => e.Especificacion).HasColumnName("especificacion");
            entity.Property(e => e.Seccion).HasMaxLength(5).HasColumnName("seccion");
            entity.Property(e => e.Obligatoria).HasColumnName("obligatoria");

            entity.HasOne(d => d.Version)
                .WithMany(p => p.Preguntas)
                .HasForeignKey(d => d.IdVersion)
                .HasConstraintName("fk_id_version");

            // Auto-referencia: especificacion → id_pregunta
            entity.HasOne(d => d.PreguntaPadre)
                .WithMany(p => p.SubPreguntas)
                .HasForeignKey(d => d.Especificacion)
                .IsRequired(false)
                .HasConstraintName("fk_especificacion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}