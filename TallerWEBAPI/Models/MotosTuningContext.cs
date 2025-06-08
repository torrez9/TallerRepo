using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TallerWEBAPI.Models;

public partial class MotosTuningContext : DbContext
{
    public MotosTuningContext()
    {
    }

    public MotosTuningContext(DbContextOptions<MotosTuningContext> options)
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
        => optionsBuilder.UseSqlServer("Data Source=BLADE15\\SQLEXPRESS;Database=MotosTuning;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("PK__Citas__394B02026B6C86F6");

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
                .HasConstraintName("FK__Citas__IdCliente__5FB337D6");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__D594664271BF2E24");

            entity.HasIndex(e => e.Correo, "UQ__Clientes__60695A19F457B8A1").IsUnique();

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Contraseña).HasMaxLength(100);
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
            entity.Property(e => e.Usuario).HasMaxLength(50);

            entity.HasMany(d => d.IdMotos).WithMany(p => p.IdClientes)
                .UsingEntity<Dictionary<string, object>>(
                    "ClienteMoto",
                    r => r.HasOne<Moto>().WithMany()
                        .HasForeignKey("IdMoto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ClienteMo__IdMot__44FF419A"),
                    l => l.HasOne<Cliente>().WithMany()
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ClienteMo__IdCli__619B8048"),
                    j =>
                    {
                        j.HasKey("IdCliente", "IdMoto").HasName("PK__ClienteM__76A88B1D6E37A7B1");
                        j.ToTable("ClienteMoto");
                    });
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PK__Compras__0A5CDB5C0FB5A6B9");

            entity.Property(e => e.FechaCompra)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MontoTotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Compras__IdProve__656C112C");
        });

        modelBuilder.Entity<CompraPieza>(entity =>
        {
            entity.HasKey(e => new { e.IdCompra, e.IdPieza }).HasName("PK__CompraPi__7E5BEEF64F3B077D");

            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.CompraPiezas)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompraPie__IdCom__6383C8BA");

            entity.HasOne(d => d.IdPiezaNavigation).WithMany(p => p.CompraPiezas)
                .HasForeignKey(d => d.IdPieza)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompraPie__IdPie__6477ECF3");
        });

        modelBuilder.Entity<DetalleReparacion>(entity =>
        {
            entity.HasKey(e => e.IdDetalleRep);

            entity.ToTable("Detalle_Reparacion");

            entity.Property(e => e.IdDetalleRep)
                .ValueGeneratedNever()
                .HasColumnName("Id_DetalleRep");
            entity.Property(e => e.CostoServicio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdReparacionesNavigation).WithMany(p => p.DetalleReparacions)
                .HasForeignKey(d => d.IdReparaciones)
                .HasConstraintName("FK_Detalle_Reparacion_Reparaciones");

            entity.HasOne(d => d.IdserviciosNavigation).WithMany(p => p.DetalleReparacions)
                .HasForeignKey(d => d.Idservicios)
                .HasConstraintName("FK_Detalle_Reparacion_Servicios");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PK__Facturas__50E7BAF11A264F69");

            entity.Property(e => e.FechaFactura)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MontoTotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Facturas__IdClie__68487DD7");

            entity.HasOne(d => d.IdReparacionNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdReparacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Facturas__IdRepa__693CA210");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.IdPieza).HasName("PK__Inventar__40735AA624B3F32C");

            entity.ToTable("Inventario");

            entity.Property(e => e.NombrePieza)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Moto>(entity =>
        {
            entity.HasKey(e => e.IdMoto).HasName("PK__Motos__33CED5FBB9179DC7");

            entity.HasIndex(e => e.Placa, "UQ__Motos__8310F99D9BBBAF32").IsUnique();

            entity.Property(e => e.Color)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EstadoEntrada).HasColumnType("text");
            entity.Property(e => e.EstadoSalida).HasColumnType("text");
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
            entity.HasKey(e => e.IdPago).HasName("PK__Pagos__FC851A3AC131BBC3");

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
                .HasConstraintName("FK__Pagos__IdFactura__6B24EA82");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__Proveedo__E8B631AFB7C6D08E");

            entity.HasIndex(e => e.Correo, "UQ__Proveedo__60695A19D4D951AD").IsUnique();

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
            entity.HasKey(e => new { e.IdReparacion, e.IdPieza }).HasName("PK__Reparaci__A8F44AAA94B89A0B");

            entity.HasOne(d => d.IdPiezaNavigation).WithMany(p => p.ReparacionPiezas)
                .HasForeignKey(d => d.IdPieza)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reparacio__IdPie__6E01572D");

            entity.HasOne(d => d.IdReparacionNavigation).WithMany(p => p.ReparacionPiezas)
                .HasForeignKey(d => d.IdReparacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reparacio__IdRep__6EF57B66");
        });

        modelBuilder.Entity<Reparacione>(entity =>
        {
            entity.HasKey(e => e.IdReparacion).HasName("PK__Reparaci__DCF37F00F89380A4");

            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("En proceso");
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
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584C8954F465");

            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio);

            entity.Property(e => e.IdServicio).ValueGeneratedNever();
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
