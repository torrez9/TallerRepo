using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TallerWEBAPI.Models;

public partial class MotosTuning3Context : DbContext
{
    public MotosTuning3Context()
    {
    }

    public MotosTuning3Context(DbContextOptions<MotosTuning3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Cita> Citas { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<CompraPieza> CompraPiezas { get; set; }

    public virtual DbSet<DetalleReparacion> DetalleReparacions { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Moto> Motos { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<ReparacionPieza> ReparacionPiezas { get; set; }

    public virtual DbSet<Reparacione> Reparaciones { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=BLADE15\\SQLEXPRESS;Database=MotosTuning3;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("PK__Citas__394B0202E562C01D");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Citas__IdCliente__6FE99F9F");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__D594664279BDBB17");

            entity.HasIndex(e => e.Correo, "UQ__Clientes__60695A1916F06CCB").IsUnique();

            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PK__Compras__0A5CDB5CBE4AA602");

            entity.Property(e => e.FechaCompra)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MontoTotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK_Compras_Proveedores");
        });

        modelBuilder.Entity<CompraPieza>(entity =>
        {
            entity.HasKey(e => e.IdCompraPieza);

            entity.Property(e => e.IdCompraPieza).ValueGeneratedNever();
            entity.Property(e => e.Importe).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.CompraPiezas)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK_CompraPiezas_Compras");

            entity.HasOne(d => d.IdPiezaNavigation).WithMany(p => p.CompraPiezas)
                .HasForeignKey(d => d.IdPieza)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompraPiezas_Inventario");
        });

        modelBuilder.Entity<DetalleReparacion>(entity =>
        {
            entity.HasKey(e => e.IdDetalleRep);

            entity.ToTable("Detalle_Reparacion");

            entity.Property(e => e.IdDetalleRep).HasColumnName("Id_DetalleRep");
            entity.Property(e => e.CantPieza).HasColumnName("Cant_pieza");
            entity.Property(e => e.CostoPieza).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CostoServicio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Importe).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdPiezaNavigation).WithMany(p => p.DetalleReparacions)
                .HasForeignKey(d => d.IdPieza)
                .HasConstraintName("FK_Detalle_Reparacion_Inventario");

            entity.HasOne(d => d.IdReparacionesNavigation).WithMany(p => p.DetalleReparacions)
                .HasForeignKey(d => d.IdReparaciones)
                .HasConstraintName("FK_Detalle_Reparacion_Reparaciones");

            entity.HasOne(d => d.IdserviciosNavigation).WithMany(p => p.DetalleReparacions)
                .HasForeignKey(d => d.Idservicios)
                .HasConstraintName("FK_Detalle_Reparacion_Servicios");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PK__Facturas__50E7BAF1EC2D2DB0");

            entity.Property(e => e.EstadoPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Estado_Pago");
            entity.Property(e => e.FechaFactura)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MontoTotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Facturas__IdClie__778AC167");

            entity.HasOne(d => d.IdReparacionNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdReparacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Facturas__IdRepa__787EE5A0");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.IdPieza).HasName("PK__Inventar__40735AA6EF9ED8DF");

            entity.ToTable("Inventario");

            entity.Property(e => e.NombrePieza)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrecioActual)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio_Actual");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK_Inventario_Proveedores");
        });

        modelBuilder.Entity<Moto>(entity =>
        {
            entity.HasKey(e => e.IdMoto).HasName("PK__Motos__33CED5FBB9179DC7");

            entity.HasIndex(e => e.Placa, "UQ__Motos__8310F99D9BBBAF32").IsUnique();

            entity.Property(e => e.Color)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Placa)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Motos)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK_Motos_Clientes");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__Pagos__FC851A3AB97F04E0");

            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdFacturaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdFactura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pagos__IdFactura__66603565");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__Proveedo__E8B631AF9AD5D702");

            entity.HasIndex(e => e.Correo, "UQ__Proveedo__60695A19F4F709EF").IsUnique();

            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ReparacionPieza>(entity =>
        {
            entity.HasKey(e => new { e.IdReparacion, e.IdPieza }).HasName("PK__Reparaci__A8F44AAA1D34936F");
        });

        modelBuilder.Entity<Reparacione>(entity =>
        {
            entity.HasKey(e => e.IdReparacion).HasName("PK__Reparaci__DCF37F00249F0F0B");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("En proceso");
            entity.Property(e => e.Fase)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdMotoNavigation).WithMany(p => p.Reparaciones)
                .HasForeignKey(d => d.IdMoto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reparacio__IdMot__4D94879B");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Reparaciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reparacio__IdUsu__4E88ABD4");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584CB651C955");

            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio);

            entity.Property(e => e.PrecioServicio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio_Servicio");
            entity.Property(e => e.Servicio1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Servicio");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF9777808F0E");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A19C7A930F1").IsUnique();

            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Usuario1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Usuario");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__IdRol__3A81B327");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
