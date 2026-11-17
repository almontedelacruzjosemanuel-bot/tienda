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

    public virtual DbSet<Categorium> Categorias { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetalleOrdenCompra> DetalleOrdenCompras { get; set; }

    public virtual DbSet<DetalleVentum> DetalleVenta { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<EspecificacionTecnica> EspecificacionTecnicas { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<OrdenCompra> OrdenCompras { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CD54BC5A33375A55");

            entity.ToTable("Categorias");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Cliente__677F38F590F6D537");

            entity.ToTable("Cliente");

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .HasColumnName("apellido");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .HasColumnName("direccion");
            entity.Property(e => e.DocumentoIdentidad)
                .HasMaxLength(30)
                .HasColumnName("documento_identidad");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<DetalleOrdenCompra>(entity =>
        {
            entity.ToTable("CompraDetalles");

            entity.HasKey(d => d.CompraDetalleId);

            entity.Property(d => d.CompraDetalleId).HasColumnName("CompraDetalleId");
            entity.Property(d => d.CompraId).HasColumnName("CompraId");
            entity.Property(d => d.ProductoId).HasColumnName("ProductoId");
            entity.Property(d => d.Cantidad).HasColumnName("Cantidad");
            entity.Property(d => d.CostoUnitario).HasColumnName("CostoUnitario");
            entity.Property(d => d.Subtotal).HasColumnName("Subtotal");

            entity.HasOne(d => d.Compra)
                .WithMany(o => o.DetalleOrdenCompras)
                .HasForeignKey(d => d.CompraId);

            entity.HasOne(d => d.Producto)
                .WithMany(p => p.DetalleOrdenCompras)
                .HasForeignKey(d => d.ProductoId);
        });

        modelBuilder.Entity<DetalleVentum>(entity =>
        {
            entity.HasKey(e => e.IdDetalleVenta).HasName("PK__DetalleV__5B265D47FCDED2B3");

            entity.Property(e => e.IdDetalleVenta).HasColumnName("id_detalle_venta");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precio_unitario");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVenta_Producto");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVenta_Venta");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__Empleado__88B51394251A265F");

            entity.ToTable("Empleado");

            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .HasColumnName("apellido");
            entity.Property(e => e.Cargo)
                .HasMaxLength(100)
                .HasColumnName("cargo");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FechaContratacion).HasColumnName("fecha_contratacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<EspecificacionTecnica>(entity =>
        {
            entity.HasKey(e => e.IdEspecificacion).HasName("PK__Especifi__052F299ED371A9FB");

            entity.ToTable("EspecificacionTecnica");

            entity.Property(e => e.IdEspecificacion).HasColumnName("id_especificacion");
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .HasColumnName("clave");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Valor)
                .HasMaxLength(255)
                .HasColumnName("valor");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.EspecificacionTecnicas)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Especificacion_Producto");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.IdMarca).HasName("PK__Marca__7E43E99E6B1806E4");

            entity.ToTable("Marca");

            entity.Property(e => e.IdMarca).HasColumnName("id_marca");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.PaisOrigen)
                .HasMaxLength(100)
                .HasColumnName("pais_origen");
            entity.Property(e => e.SitioWeb)
                .HasMaxLength(255)
                .HasColumnName("sitio_web");
        });

        modelBuilder.Entity<OrdenCompra>(entity =>
        {
            entity.ToTable("Compras");

            entity.HasKey(o => o.CompraId);

            entity.Property(o => o.CompraId).HasColumnName("CompraId");
            entity.Property(o => o.IdProveedor).HasColumnName("ProveedorId");
            entity.Property(o => o.IdEmpleado).HasColumnName("EmpleadoId");
            entity.Property(o => o.FechaOrden).HasColumnName("FechaOrden");
            entity.Property(o => o.Total).HasColumnName("Total");
            entity.Property(o => o.Estado).HasColumnName("Estado");

            entity.HasOne(o => o.IdEmpleadoNavigation)
                .WithMany(e => e.OrdenCompras)
                .HasForeignKey(o => o.IdEmpleado);

            entity.HasOne(o => o.IdProveedorNavigation)
                .WithMany(p => p.OrdenCompras)
                .HasForeignKey(o => o.IdProveedor);
            });

        
    modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Producto");

            entity.HasKey(p => p.ProductoId);

            entity.Property(p => p.ProductoId).HasColumnName("ProductoId");
            entity.Property(p => p.CategoriaId).HasColumnName("CategoriaId");
            entity.Property(p => p.MarcaId).HasColumnName("MarcaId");
            entity.Property(p => p.Sku).HasColumnName("Sku");
            entity.Property(p => p.Nombre).HasColumnName("Nombre");
            entity.Property(p => p.Descripcion).HasColumnName("Descripcion");
            entity.Property(p => p.Costo).HasColumnName("Costo");
            entity.Property(p => p.Precio).HasColumnName("Precio");
            entity.Property(p => p.Stock).HasColumnName("Stock");
            entity.Property(p => p.PuntoReorden).HasColumnName("PuntoReorden");
            entity.Property(p => p.Garantia).HasColumnName("Garantia");
            entity.Property(p => p.MargenGanancia).HasColumnName("MargenGanancia");
            entity.Property(p => p.Itbis).HasColumnName("Itbis");

            entity.HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId);

            entity.HasOne(p => p.Marca)
                .WithMany(m => m.Productos)
                .HasForeignKey(p => p.MarcaId);
        });


        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__Proveedo__8D3DFE286F5CB816");

            entity.ToTable("Proveedor");

            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.ContactoNombre)
                .HasMaxLength(150)
                .HasColumnName("contacto_nombre");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.NombreEmpresa)
                .HasMaxLength(150)
                .HasColumnName("nombre_empresa");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__Venta__459533BF249E90C6");

            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasColumnName("estado");
            entity.Property(e => e.FechaVenta).HasColumnName("fecha_venta");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .HasColumnName("metodo_pago");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_Cliente");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_Empleado");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
