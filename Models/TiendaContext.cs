
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
    public virtual DbSet<CompraDetalle> CompraDetalles { get; set; }
    public virtual DbSet<Compra> Compras { get; set; }
    public virtual DbSet<Empleado> Empleados { get; set; }
    public virtual DbSet<EspecificacionTecnica> EspecificacionesTecnicas { get; set; }
    public virtual DbSet<Marca> Marcas { get; set; }
    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }
    public virtual DbSet<Producto> Productos { get; set; }
    public virtual DbSet<Proveedor> Proveedores { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<VentaDetalle> VentaDetalles { get; set; }
    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("Categorias");
            entity.HasKey(e => e.CategoriaId);

            entity.Property(e => e.CategoriaId).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.Descripcion).HasMaxLength(510);
        });

        modelBuilder.Entity<Ciudad>(entity =>
        {
            entity.ToTable("Ciudades");
            entity.HasKey(e => e.CiudadId);

            entity.Property(e => e.CiudadId).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Clientes");
            entity.HasKey(e => e.ClienteId);

            entity.Property(e => e.ClienteId).ValueGeneratedOnAdd();
            entity.Property(e => e.DocumentoIdentidad).HasMaxLength(60);
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.Telefono).HasMaxLength(40);
            entity.Property(e => e.Direccion).HasMaxLength(510);
            entity.Property(e => e.FechaRegistro).HasColumnType("date");
            entity.Property(e => e.NombreCompleto).HasMaxLength(300);

            entity.HasOne(e => e.Ciudad)
                .WithMany(e => e.Clientes)
                .HasForeignKey(e => e.CiudadId);
        });

        modelBuilder.Entity<CompraDetalle>(entity =>
        {
            entity.ToTable("CompraDetalles");
            entity.HasKey(e => e.CompraDetalleId);

            entity.Property(e => e.CompraDetalleId).ValueGeneratedOnAdd();
            entity.Property(e => e.CostoUnitario).HasColumnType("money");
            entity.Property(e => e.Subtotal).HasColumnType("money");

            entity.HasOne(e => e.Compra)
                .WithMany(e => e.CompraDetalles)
                .HasForeignKey(e => e.CompraId);

            entity.HasOne(e => e.Producto)
                .WithMany(e => e.CompraDetalles)
                .HasForeignKey(e => e.ProductoId);
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.ToTable("Compras");
            entity.HasKey(e => e.CompraId);

            entity.Property(e => e.CompraId).ValueGeneratedOnAdd();
            entity.Property(e => e.Fecha).HasColumnType("datetime2");
            entity.Property(e => e.Total).HasColumnType("money");
            entity.Property(e => e.Estado).HasMaxLength(100);

            entity.HasOne(e => e.Empleado)
                .WithMany(e => e.Compras)
                .HasForeignKey(e => e.EmpleadoId);

            entity.HasOne(e => e.Proveedor)
                .WithMany(e => e.Compras)
                .HasForeignKey(e => e.ProveedorId);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.ToTable("Empleados");
            entity.HasKey(e => e.EmpleadoId);

            entity.Property(e => e.EmpleadoId).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.Apellido).HasMaxLength(200);
            entity.Property(e => e.Cargo).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.Telefono).HasMaxLength(40);
            entity.Property(e => e.FechaContratacion).HasColumnType("date");
        });

        modelBuilder.Entity<EspecificacionTecnica>(entity =>
        {
            entity.ToTable("EspecificacionesTecnicas");
            entity.HasKey(e => e.EspecificacionId);

            entity.Property(e => e.EspecificacionId).ValueGeneratedOnAdd();
            entity.Property(e => e.Clave).HasMaxLength(200);
            entity.Property(e => e.Valor).HasMaxLength(510);

            entity.HasOne(e => e.Producto)
                .WithMany(e => e.EspecificacionesTecnicas)
                .HasForeignKey(e => e.ProductoId);
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.ToTable("Marcas");
            entity.HasKey(e => e.MarcaId);

            entity.Property(e => e.MarcaId).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.PaisOrigen).HasMaxLength(200);
            entity.Property(e => e.SitioWeb).HasMaxLength(510);
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.ToTable("MetodoPagos");
            entity.HasKey(e => e.MetodoPagoId);

            entity.Property(e => e.MetodoPagoId).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Productos");
            entity.HasKey(e => e.ProductoId);

            entity.Property(e => e.ProductoId).ValueGeneratedOnAdd();
            entity.Property(e => e.Sku).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(300);
            entity.Property(e => e.Descripcion).HasColumnType("nvarchar(max)");
            entity.Property(e => e.Costo).HasColumnType("money");
            entity.Property(e => e.Precio).HasColumnType("money");
            entity.Property(e => e.Garantia).HasMaxLength(100);
            entity.Property(e => e.MargenGanancia).HasPrecision(5, 2);
            entity.Property(e => e.Itbis).HasColumnType("money");

            entity.HasOne(e => e.Categoria)
                .WithMany(e => e.Productos)
                .HasForeignKey(e => e.CategoriaId);

            entity.HasOne(e => e.Marca)
                .WithMany(e => e.Productos)
                .HasForeignKey(e => e.MarcaId);
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.ToTable("Proveedores");
            entity.HasKey(e => e.ProveedorId);

            entity.Property(e => e.ProveedorId).ValueGeneratedOnAdd();
            entity.Property(e => e.NombreEmpresa).HasMaxLength(300);
            entity.Property(e => e.ContactoNombre).HasMaxLength(300);
            entity.Property(e => e.Telefono).HasMaxLength(40);
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.Direccion).HasMaxLength(510);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.UsuarioId);

            entity.Property(e => e.UsuarioId).ValueGeneratedOnAdd();
            entity.Property(e => e.NombreUsuario).HasMaxLength(200).IsRequired();
            entity.Property(e => e.ContrasenaHash).HasMaxLength(510).IsRequired();
            entity.Property(e => e.Activo).IsRequired();

            entity.HasOne(e => e.Empleado)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(e => e.EmpleadoId);
        });

        modelBuilder.Entity<VentaDetalle>(entity =>
        {
            entity.ToTable("VentaDetalles");
            entity.HasKey(e => e.VentaDetalleId);

            entity.Property(e => e.VentaDetalleId).ValueGeneratedOnAdd();
            entity.Property(e => e.Precio).HasColumnType("money");
            entity.Property(e => e.Costo).HasColumnType("money");
            entity.Property(e => e.Itbis).HasColumnType("money");

            entity.HasOne(e => e.Venta)
                .WithMany(e => e.VentaDetalles)
                .HasForeignKey(e => e.VentaId);

            entity.HasOne(e => e.Producto)
                .WithMany(e => e.VentaDetalles)
                .HasForeignKey(e => e.ProductoId);
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.ToTable("Ventas");
            entity.HasKey(e => e.VentaId);

            entity.Property(e => e.VentaId).ValueGeneratedOnAdd();
            entity.Property(e => e.Fecha).HasColumnType("datetime2").IsRequired();
            entity.Property(e => e.Total).HasColumnType("money");
            entity.Property(e => e.Subtotal).HasColumnType("money");
            entity.Property(e => e.Itbis).HasColumnType("money");
            entity.Property(e => e.ComprobanteFiscal).HasMaxLength(100);

            entity.HasOne(e => e.Cliente)
                .WithMany(e => e.Ventas)
                .HasForeignKey(e => e.ClienteId);

            entity.HasOne(e => e.Empleado)
                .WithMany(e => e.Ventas)
                .HasForeignKey(e => e.EmpleadoId);

            entity.HasOne(e => e.MetodoPago)
                .WithMany(e => e.Ventas)
                .HasForeignKey(e => e.MetodoPagoId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
