using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EncuestasApi.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dispositivo> Dispositivos { get; set; }

    public virtual DbSet<Encuesta> Encuestas { get; set; }

    public virtual DbSet<EncuestasRespondida> EncuestasRespondidas { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<Opcione> Opciones { get; set; }

    public virtual DbSet<Pregunta> Preguntas { get; set; }

    public virtual DbSet<Respuesta> Respuestas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Seccione> Secciones { get; set; }

    public virtual DbSet<Subopcione> Subopciones { get; set; }

    public virtual DbSet<TiposControle> TiposControles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuariosRole> UsuariosRoles { get; set; }

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

        modelBuilder.Entity<Encuesta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("encuestas_pkey");

            entity.ToTable("encuestas");

            entity.HasIndex(e => e.UsuarioId, "fki_fk_usuario");

            entity.HasIndex(e => e.UsuarioId, "fki_usuario_encuesta_fk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaCierre).HasColumnName("fecha_cierre");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.Titulo)
                .HasMaxLength(75)
                .HasColumnName("titulo");
            entity.Property(e => e.UltimaActualizacion).HasColumnName("ultima_actualizacion");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Encuesta)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario");
        });

        modelBuilder.Entity<EncuestasRespondida>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("encuestas_respondidas_pkey");

            entity.ToTable("encuestas_respondidas");

            entity.HasIndex(e => new { e.EncuestaId, e.Curp }, "uq_encuesta_curp").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Curp)
                .HasMaxLength(18)
                .HasColumnName("curp");
            entity.Property(e => e.DispositivoId).HasColumnName("dispositivo_id");
            entity.Property(e => e.EncuestaId).HasColumnName("encuesta_id");
            entity.Property(e => e.FechaFin)
                .HasColumnType("time with time zone")
                .HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha inicio");
            entity.Property(e => e.FechaSincronizacion)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_sincronizacion");
            entity.Property(e => e.Latitud).HasColumnName("latitud");
            entity.Property(e => e.LocalId).HasColumnName("local_id");
            entity.Property(e => e.Longitud).HasColumnName("longitud");
            entity.Property(e => e.Sincronizada)
                .HasDefaultValue(false)
                .HasColumnName("sincronizada");
            entity.Property(e => e.Terminada)
                .HasDefaultValue(false)
                .HasColumnName("terminada");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Dispositivo).WithMany(p => p.EncuestasRespondida)
                .HasForeignKey(d => d.DispositivoId)
                .HasConstraintName("fk_dispositivo");

            entity.HasOne(d => d.Encuesta).WithMany(p => p.EncuestasRespondida)
                .HasForeignKey(d => d.EncuestaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_encuesta");

            entity.HasOne(d => d.Usuario).WithMany(p => p.EncuestasRespondida)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("fk_usuario");
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("estatus_pkey");

            entity.ToTable("estatus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("character varying")
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Opcione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("opciones_pkey");

            entity.ToTable("opciones");

            entity.HasIndex(e => e.PreguntaId, "fki_fk_pregunta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConSubopciones)
                .HasDefaultValue(false)
                .HasColumnName("con_subopciones");
            entity.Property(e => e.Placeholder)
                .HasMaxLength(50)
                .HasColumnName("placeholder");
            entity.Property(e => e.PreguntaId).HasColumnName("pregunta_id");
            entity.Property(e => e.Texto)
                .HasMaxLength(50)
                .HasColumnName("texto");
            entity.Property(e => e.TipoControlId).HasColumnName("tipo_control_id");

            entity.HasOne(d => d.Pregunta).WithMany(p => p.Opciones)
                .HasForeignKey(d => d.PreguntaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_pregunta");

            entity.HasOne(d => d.TipoControl).WithMany(p => p.Opciones)
                .HasForeignKey(d => d.TipoControlId)
                .HasConstraintName("opciones_tipo_control_id_fkey");
        });

        modelBuilder.Entity<Pregunta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("preguntas_pkey");

            entity.ToTable("preguntas");

            entity.HasIndex(e => e.UsuarioId, "fki_fk_estatus");

            entity.HasIndex(e => e.SeccionId, "fki_fk_seccion");

            entity.HasIndex(e => e.TipoControlId, "fki_fk_tipo_control");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Agregar)
                .HasDefaultValue(false)
                .HasColumnName("agregar");
            entity.Property(e => e.ConOpciones)
                .HasDefaultValue(false)
                .HasColumnName("con_opciones");
            entity.Property(e => e.EncuestaId).HasColumnName("encuesta_id");
            entity.Property(e => e.EstatusId).HasColumnName("estatus_id");
            entity.Property(e => e.Orden).HasColumnName("orden");
            entity.Property(e => e.Placeholder)
                .HasMaxLength(100)
                .HasColumnName("placeholder");
            entity.Property(e => e.Requerido).HasColumnName("requerido");
            entity.Property(e => e.SeccionId).HasColumnName("seccion_id");
            entity.Property(e => e.Texto)
                .HasMaxLength(200)
                .HasColumnName("texto");
            entity.Property(e => e.TipoControlId).HasColumnName("tipo_control_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Encuesta).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.EncuestaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_encuesta");

            entity.HasOne(d => d.Seccion).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.SeccionId)
                .HasConstraintName("fk_seccion");

            entity.HasOne(d => d.TipoControl).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.TipoControlId)
                .HasConstraintName("fk_tipo_control");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_estatus");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario");
        });

        modelBuilder.Entity<Respuesta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("respuestas_pkey");

            entity.ToTable("respuestas");

            entity.HasIndex(e => e.EncuestaRespondidaId, "fki_fk_encuesta_respondida");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DetalleRespuesta)
                .HasColumnType("character varying")
                .HasColumnName("detalle_respuesta");
            entity.Property(e => e.EncuestaRespondidaId).HasColumnName("encuesta_respondida_id");
            entity.Property(e => e.OpcionId).HasColumnName("opcion_id");
            entity.Property(e => e.PreguntaId).HasColumnName("pregunta_id");
            entity.Property(e => e.SubopcionId).HasColumnName("subopcion_id");

            entity.HasOne(d => d.EncuestaRespondida).WithMany(p => p.Respuesta)
                .HasForeignKey(d => d.EncuestaRespondidaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_encuesta_respondida");

            entity.HasOne(d => d.Pregunta).WithMany(p => p.Respuesta)
                .HasForeignKey(d => d.PreguntaId)
                .HasConstraintName("fk_pregunta");

            entity.HasOne(d => d.Subopcion).WithMany(p => p.Respuesta)
                .HasForeignKey(d => d.SubopcionId)
                .HasConstraintName("respuestas_subopcion_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Seccione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secciones_pkey");

            entity.ToTable("secciones");

            entity.HasIndex(e => e.EncuestaId, "fki_fk_encuesta");

            entity.HasIndex(e => e.EncuestaId, "fki_seccion_encuesta_fk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EncuestaId).HasColumnName("encuesta_id");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .HasColumnName("titulo");

            entity.HasOne(d => d.Encuesta).WithMany(p => p.Secciones)
                .HasForeignKey(d => d.EncuestaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_encuesta");
        });

        modelBuilder.Entity<Subopcione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("subopciones_pkey");

            entity.ToTable("subopciones");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConFecha)
                .HasDefaultValue(false)
                .HasColumnName("con_fecha");
            entity.Property(e => e.ConObservaciones)
                .HasDefaultValue(false)
                .HasColumnName("con_observaciones");
            entity.Property(e => e.OpcionId).HasColumnName("opcion_id");
            entity.Property(e => e.Texto)
                .HasMaxLength(100)
                .HasColumnName("texto");
            entity.Property(e => e.TipoControlId).HasColumnName("tipo_control_id");

            entity.HasOne(d => d.Opcion).WithMany(p => p.Subopciones)
                .HasForeignKey(d => d.OpcionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("subopciones_opcion_id_fkey");

            entity.HasOne(d => d.TipoControl).WithMany(p => p.Subopciones)
                .HasForeignKey(d => d.TipoControlId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("subopciones_tipo_control_id_fkey");
        });

        modelBuilder.Entity<TiposControle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tipos_controles_pkey");

            entity.ToTable("tipos_controles");

            entity.Property(e => e.Tipo)
                .HasColumnType("character varying")
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FullName)
                .HasMaxLength(75)
                .HasColumnName("full_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(50)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UsuariosRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_roles_pkey");

            entity.ToTable("usuarios_roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Rol).WithMany(p => p.UsuariosRoles)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("fk_rol");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuariosRoles)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("fk_usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
