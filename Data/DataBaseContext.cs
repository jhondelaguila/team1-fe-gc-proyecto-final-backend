using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using team1_fe_gc_proyecto_final_backend.Models;

namespace team1_fe_gc_proyecto_final_backend.Data;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actividad> Actividades { get; set; }

    public virtual DbSet<Alojamiento> Alojamientos { get; set; }

    public virtual DbSet<Direccion> Direcciones { get; set; }

    public virtual DbSet<Favorito> Favoritos { get; set; }

    public virtual DbSet<Imagen> Imagenes { get; set; }

    public virtual DbSet<Nivel> Niveles { get; set; }

    public virtual DbSet<Oferta> Ofertas { get; set; }

    public virtual DbSet<OfertasActividades> OfertasActividades { get; set; }

    public virtual DbSet<OfertasImagenes> OfertasImagenes { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<ServiciosAlojamientos> ServiciosAlojamientos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Actividad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("actividades");

            entity.HasIndex(e => e.IdDireccion, "id_direccion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .HasColumnName("titulo");

            //entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Actividades)
            //    .HasForeignKey(d => d.IdDireccion)
            //    .HasConstraintName("actividades_ibfk_1");
        });

        modelBuilder.Entity<Alojamiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("alojamientos");

            entity.HasIndex(e => e.IdDireccion, "id_direccion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria).HasColumnName("categoria");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(9)
                .HasColumnName("telefono");

            //entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Alojamientos)
            //    .HasForeignKey(d => d.IdDireccion)
            //    .HasConstraintName("alojamientos_ibfk_1");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("direcciones");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Calle)
                .HasMaxLength(50)
                .HasColumnName("calle");
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(5)
                .HasColumnName("codigo_postal");
            entity.Property(e => e.Localidad)
                .HasMaxLength(50)
                .HasColumnName("localidad");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Pais)
                .HasMaxLength(50)
                .HasColumnName("pais");
            entity.Property(e => e.Provincia)
                .HasMaxLength(50)
                .HasColumnName("provincia");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("favoritos");

            entity.HasIndex(e => e.IdOferta, "id_oferta");

            entity.HasIndex(e => e.IdUsuario, "id_usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdOferta).HasColumnName("id_oferta");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            //entity.HasOne(d => d.IdOfertaNavigation).WithMany(p => p.Favoritos)
            //    .HasForeignKey(d => d.IdOferta)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("favoritos_ibfk_1");

            //entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Favoritos)
            //    .HasForeignKey(d => d.IdUsuario)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("favoritos_ibfk_2");
        });

        modelBuilder.Entity<Imagen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("imagenes");

            entity.HasIndex(e => e.IdActividad, "id_actividad");

            entity.HasIndex(e => e.IdAlojamiento, "id_alojamiento");

            entity.HasIndex(e => e.Url, "url").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdActividad).HasColumnName("id_actividad");
            entity.Property(e => e.IdAlojamiento).HasColumnName("id_alojamiento");
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(40)
                .HasColumnName("ubicacion");
            entity.Property(e => e.Url)
                .HasMaxLength(150)
                .HasColumnName("url");

            //entity.HasOne(d => d.IdActividadNavigation).WithMany(p => p.Imagenes)
            //    .HasForeignKey(d => d.IdActividad)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("imagenes_ibfk_1");

            //entity.HasOne(d => d.IdAlojamientoNavigation).WithMany(p => p.Imagenes)
            //    .HasForeignKey(d => d.IdAlojamiento)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("imagenes_ibfk_2");
        });

        modelBuilder.Entity<Nivel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("niveles");

            entity.HasIndex(e => e.Nombre, "nombre").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExperienciaMinima).HasColumnName("experiencia_minima");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .HasColumnName("nombre");
            entity.Property(e => e.Ventajas)
                .HasMaxLength(500)
                .HasColumnName("ventajas");
        });

        modelBuilder.Entity<Oferta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ofertas");

            entity.HasIndex(e => e.IdAlojamiento, "id_alojamiento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.IdAlojamiento).HasColumnName("id_alojamiento");
            entity.Property(e => e.MaxPersonas).HasColumnName("max_personas");
            entity.Property(e => e.OfertasDisponibles).HasColumnName("ofertas_disponibles");
            entity.Property(e => e.Precio).HasColumnName("precio");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .HasColumnName("titulo");

            //entity.HasOne(d => d.IdAlojamientoNavigation).WithMany(p => p.Oferta)
            //    .HasForeignKey(d => d.IdAlojamiento)
            //    .HasConstraintName("ofertas_ibfk_1");
        });

        modelBuilder.Entity<OfertasActividades>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ofertas_actividades");

            entity.HasIndex(e => e.IdActividad, "id_actividad");

            entity.HasIndex(e => e.IdOferta, "id_oferta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdActividad).HasColumnName("id_actividad");
            entity.Property(e => e.IdOferta).HasColumnName("id_oferta");

            //entity.HasOne(d => d.IdActividadNavigation).WithMany(p => p.OfertasActividades)
            //    .HasForeignKey(d => d.IdActividad)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("ofertas_actividades_ibfk_2");

            //entity.HasOne(d => d.IdOfertaNavigation).WithMany(p => p.OfertasActividades)
            //    .HasForeignKey(d => d.IdOferta)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("ofertas_actividades_ibfk_1");
        });

        modelBuilder.Entity<OfertasImagenes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ofertas_imagenes");

            entity.HasIndex(e => e.IdImagen, "id_imagen");

            entity.HasIndex(e => e.IdOferta, "id_oferta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdImagen).HasColumnName("id_imagen");
            entity.Property(e => e.IdOferta).HasColumnName("id_oferta");

            //entity.HasOne(d => d.IdImagenNavigation).WithMany(p => p.OfertasImagenes)
            //    .HasForeignKey(d => d.IdImagen)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("ofertas_imagenes_ibfk_1");

            //entity.HasOne(d => d.IdOfertaNavigation).WithMany(p => p.OfertasImagenes)
            //    .HasForeignKey(d => d.IdOferta)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("ofertas_imagenes_ibfk_2");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reservas");

            entity.HasIndex(e => e.IdOferta, "id_oferta");

            entity.HasIndex(e => e.IdUsuario, "id_usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasColumnType("enum('Completada','Activa','Cancelada')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.IdOferta).HasColumnName("id_oferta");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            //entity.HasOne(d => d.IdOfertaNavigation).WithMany(p => p.Reservas)
            //    .HasForeignKey(d => d.IdOferta)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("reservas_ibfk_1");

            //entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Reservas)
            //    .HasForeignKey(d => d.IdUsuario)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("reservas_ibfk_2");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("servicios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<ServiciosAlojamientos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("servicios_alojamientos");

            entity.HasIndex(e => e.IdAlojamiento, "id_alojamiento");

            entity.HasIndex(e => e.IdServicio, "id_servicio");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdAlojamiento).HasColumnName("id_alojamiento");
            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");

            //entity.HasOne(d => d.IdAlojamientoNavigation).WithMany(p => p.ServiciosAlojamientos)
            //    .HasForeignKey(d => d.IdAlojamiento)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("servicios_alojamientos_ibfk_2");

            //entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.ServiciosAlojamientos)
            //    .HasForeignKey(d => d.IdServicio)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("servicios_alojamientos_ibfk_1");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.IdDireccion, "id_direccion");

            entity.HasIndex(e => e.IdNivel, "id_nivel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Admin).HasColumnName("admin").HasDefaultValueSql("false");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .HasColumnName("apellidos").HasDefaultValueSql("null");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Experiencia)
                .HasDefaultValueSql("'0'")
                .HasColumnName("experiencia");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento").HasDefaultValueSql("null");
            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion").HasDefaultValueSql("null");
            entity.Property(e => e.IdNivel).HasColumnName("id_nivel").HasDefaultValueSql("null");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Pass)
                .HasMaxLength(512)
                .HasColumnName("pass");
            entity.Property(e => e.Puntos)
                .HasDefaultValueSql("'0'")
                .HasColumnName("puntos");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .HasColumnName("telefono").HasDefaultValueSql("null");

            //entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Usuarios)
            //    .HasForeignKey(d => d.IdDireccion)
            //    .HasConstraintName("usuarios_ibfk_2");

            //entity.HasOne(d => d.IdNivelNavigation).WithMany(p => p.Usuarios)
            //    .HasForeignKey(d => d.IdNivel)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("usuarios_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
