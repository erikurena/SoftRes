using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using restaurante.Models;

namespace restaurante.dbContext;

public partial class DbrestauranteContext : DbContext
{
    public DbrestauranteContext()
    {
    }

    public DbrestauranteContext(DbContextOptions<DbrestauranteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<Categoriacomplemento> Categoriacomplementos { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Complemento> Complementos { get; set; }

    public virtual DbSet<Detallecomplemento> Detallecomplementos { get; set; }

    public virtual DbSet<Detalleorden> Detalleordens { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Orden> Ordens { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");      

        modelBuilder.Entity<Categoriacomplemento>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaComplemento).HasName("PRIMARY");

            entity
                .ToTable("categoriacomplemento")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.TipoCategoriaComplemento).HasMaxLength(100);
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");

            entity
                .ToTable("categoria")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.TipoCategoria).HasMaxLength(50);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PRIMARY");

            entity
                .ToTable("cliente")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Apellido).HasMaxLength(60);
            entity.Property(e => e.Cedula).HasMaxLength(45);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(45);
        });

        modelBuilder.Entity<Complemento>(entity =>
        {
            entity.HasKey(e => e.IdComplemento).HasName("PRIMARY");

            entity
                .ToTable("complemento")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.IdCategoriaComplemento, "fk_complemento_categoriacomplemento1_idx");

            entity.Property(e => e.NombreIngrediente).HasMaxLength(100);

            entity.HasOne(d => d.IdCategoriaComplementoNavigation).WithMany(p => p.Complementos)
                .HasForeignKey(d => d.IdCategoriaComplemento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_complemento_categoriacomplemento1");
        });

        modelBuilder.Entity<Detallecomplemento>(entity =>
        {
            entity.HasKey(e => e.IdDetalleProducto).HasName("PRIMARY");

            entity
                .ToTable("detallecomplemento")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.IdComplemento, "fk_detalleProducto_ingrediente1_idx");

            entity.HasIndex(e => e.IdDetalleOrden, "fk_detalleproducto_detalleorden1_idx");

            entity.HasOne(d => d.IdComplementoNavigation).WithMany(p => p.Detallecomplementos)
                .HasForeignKey(d => d.IdComplemento)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_detalleProducto_ingrediente1");

            entity.HasOne(d => d.IdDetalleOrdenNavigation).WithMany(p => p.Detallecomplementos)
                .HasForeignKey(d => d.IdDetalleOrden)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_detalleproducto_detalleorden1");
        });

        modelBuilder.Entity<Detalleorden>(entity =>
        {
            entity.HasKey(e => e.IdDetalleOrden).HasName("PRIMARY");

            entity
                .ToTable("detalleorden")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.IdOrden, "fk_detalleorden_orden1_idx");

            entity.HasIndex(e => e.IdProducto, "fk_detalleorden_producto1_idx");

            entity.Property(e => e.PrecioUnitario).HasPrecision(10);
            entity.Property(e => e.SubTotal).HasPrecision(10);

            entity.HasOne(d => d.IdOrdenNavigation).WithMany(p => p.Detalleordens)
                .HasForeignKey(d => d.IdOrden)
                .HasConstraintName("fk_detalleorden_orden1");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Detalleordens)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("fk_detalleorden_producto1");
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__efmigrationshistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.IdCargo).HasName("PRIMARY");

            entity
                .ToTable("cargo")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.TipoCargo).HasMaxLength(60);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PRIMARY");

            entity
                .ToTable("empleado")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.IdCargo, "fk_empleado_cargo1_idx");

            entity.Property(e => e.ApellidoMaterno).HasMaxLength(45);
            entity.Property(e => e.ApellidoPaterno).HasMaxLength(45);
            entity.Property(e => e.FechaContratacion).HasMaxLength(45);
            entity.Property(e => e.FotoEmpleado).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(45);
            entity.Property(e => e.Pass).HasMaxLength(200);
            entity.Property(e => e.Telefono).HasMaxLength(45);

            entity.HasOne(d => d.IdCargoNavigation)
                        .WithMany(p => p.Empleados)
                        .HasForeignKey(d => d.IdCargo)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_empleado_cargo1");
        });

        modelBuilder.Entity<Orden>(entity =>
        {
            entity.HasKey(e => e.IdOrden).HasName("PRIMARY");

            entity
                .ToTable("orden")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.IdCliente, "fk_orden_cliente_idx");

            entity.HasIndex(e => e.IdEmpleado, "fk_orden_empleado1_idx");

            entity.Property(e => e.Descuento).HasPrecision(10);
            entity.Property(e => e.TiempoOrden).HasColumnType("time");
            entity.Property(e => e.Total).HasPrecision(10);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Ordens)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orden_cliente");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Ordens)
                .HasForeignKey(d => d.IdEmpleado)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orden_empleado1");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PRIMARY");

            entity
                .ToTable("producto")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.IdCategoria, "fk_producto_categoria1_idx");

            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.FotoProducto).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Precio).HasPrecision(10);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_producto_categoria1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
