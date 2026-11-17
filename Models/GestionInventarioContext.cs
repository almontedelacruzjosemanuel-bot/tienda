using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace tienda.Models;

public partial class TiendaContext : DbContext
{
    public TiendaContext()
    {
    }

    public TiendaContext(DbContextOptions<TiendaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Ciudad> Ciudades { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<CompraDetalle> CompraDetalles { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<EspecificacionesTecnica> EspecificacionesTecnicas { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedor> Proveedores { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<VentaDetalle> VentaDetalles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=gestion_inventario;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__CD54BC5A33375A55");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Ciudad>(entity =>
        {
            entity.HasKey(e => e.CiudadId).HasName("PK__Ciudades__E826E77095C089C5");

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Cliente__677F38F590F6D537");

            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.DocumentoIdentidad).HasMaxLength(30);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.Ciudad).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.CiudadId)
                .HasConstraintName("FK_Clientes_Ciudades");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.CompraId).HasName("PK__OrdenCom__71B729AF094C83D9");

            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Total).HasColumnType("money");

            entity.HasOne(d => d.Empleado).WithMany(p => p.Compras)
                .HasForeignKey(d => d.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenCompra_Empleado");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Compras)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenCompra_Proveedor");
        });

        modelBuilder.Entity<CompraDetalle>(entity =>
        {
            entity.HasKey(e => e.CompraDetalleId).HasName("PK__DetalleO__BD16E279F7A6B0F0");

            entity.Property(e => e.CostoUnitario).HasColumnType("money");
            entity.Property(e => e.Subtotal).HasColumnType("money");

            entity.HasOne(d => d.Compra).WithMany(p => p.CompraDetalles)
                .HasForeignKey(d => d.CompraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleCompra_OrdenCompra");

            entity.HasOne(d => d.Producto).WithMany(p => p.CompraDetalles)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleCompra_Producto");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.EmpleadoId).HasName("PK__Empleado__88B51394251A265F");

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Cargo).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<EspecificacionesTecnica>(entity =>
        {
            entity.HasKey(e => e.EspecificacionId).HasName("PK__Especifi__052F299ED371A9FB");

            entity.Property(e => e.Clave).HasMaxLength(100);
            entity.Property(e => e.Valor).HasMaxLength(255);

            entity.HasOne(d => d.Producto).WithMany(p => p.EspecificacionesTecnicas)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Especificacion_Producto");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.MarcaId).HasName("PK__Marca__7E43E99E6B1806E4");

            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PaisOrigen).HasMaxLength(100);
            entity.Property(e => e.SitioWeb).HasMaxLength(255);
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.MetodoPagoId).HasName("PK__MetodoPa__A8FEAF5476A5B007");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__FF341C0D25914A49");

            entity.Property(e => e.Costo).HasColumnType("money");
            entity.Property(e => e.Garantia).HasMaxLength(50);
            entity.Property(e => e.Itbis).HasColumnType("money");
            entity.Property(e => e.MargenGanancia).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.Precio).HasColumnType("money");
            entity.Property(e => e.Sku).HasMaxLength(50);

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");

            entity.HasOne(d => d.Marca).WithMany(p => p.Productos)
                .HasForeignKey(d => d.MarcaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Marca");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.ProveedorId).HasName("PK__Proveedo__8D3DFE286F5CB816");

            entity.Property(e => e.ContactoNombre).HasMaxLength(150);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.NombreEmpresa).HasMaxLength(150);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7B8B0D7E09B");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ContrasenaHash).HasMaxLength(255);
            entity.Property(e => e.NombreUsuario).HasMaxLength(100);

            entity.HasOne(d => d.Empleado).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Empleados");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.VentaId).HasName("PK__Venta__459533BF249E90C6");

            entity.Property(e => e.ComprobanteFiscal).HasMaxLength(50);
            entity.Property(e => e.Itbis).HasColumnType("money");
            entity.Property(e => e.Subtotal).HasColumnType("money");
            entity.Property(e => e.Total).HasColumnType("money");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Venta)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_Cliente");

            entity.HasOne(d => d.Empleado).WithMany(p => p.Venta)
                .HasForeignKey(d => d.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_Empleado");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.Venta)
                .HasForeignKey(d => d.MetodoPagoId)
                .HasConstraintName("FK_Ventas_MetodoPagos");
        });

        modelBuilder.Entity<VentaDetalle>(entity =>
        {
            entity.HasKey(e => e.VentaDetalleId).HasName("PK__DetalleV__5B265D47FCDED2B3");

            entity.Property(e => e.Costo).HasColumnType("money");
            entity.Property(e => e.Itbis).HasColumnType("money");
            entity.Property(e => e.Precio).HasColumnType("money");

            entity.HasOne(d => d.Producto).WithMany(p => p.VentaDetalles)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVenta_Producto");

            entity.HasOne(d => d.Venta).WithMany(p => p.VentaDetalles)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVenta_Venta");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
