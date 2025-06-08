using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TallerWEBAPI.Datos;

public partial class DbTiendaSeptentrionContext : DbContext
{
    public DbTiendaSeptentrionContext()
    {
    }

    public DbTiendaSeptentrionContext(DbContextOptions<DbTiendaSeptentrionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AperturaCaja> AperturaCajas { get; set; }

    public virtual DbSet<ArqueoCaja> ArqueoCajas { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }

    public virtual DbSet<DetalleCredito> DetalleCreditos { get; set; }

    public virtual DbSet<DetalleDeVentum> DetalleDeVenta { get; set; }

    public virtual DbSet<Devolucion> Devolucions { get; set; }

    public virtual DbSet<Egreso> Egresos { get; set; }

    public virtual DbSet<FacturaCredito> FacturaCreditos { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<OtrasSalidasDeInventario> OtrasSalidasDeInventarios { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<TasaDeCambio> TasaDeCambios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    public virtual DbSet<VistaArqueoCajaPorPeriodoCajero> VistaArqueoCajaPorPeriodoCajeros { get; set; }

    public virtual DbSet<VistaInventarioActual> VistaInventarioActuals { get; set; }

    public virtual DbSet<VistaSalidasInventarioPorPeriodoMotivo> VistaSalidasInventarioPorPeriodoMotivos { get; set; }

    public virtual DbSet<VistaStockProximoAgotarse> VistaStockProximoAgotarses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=BLADE15\\SQLEXPRESS;Database=DB_Tienda_Septentrion;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AperturaCaja>(entity =>
        {
            entity.HasKey(e => e.IdApertura).HasName("PK__Apertura__1DCB12E5264D3CC6");

            entity.ToTable("Apertura caja");

            entity.Property(e => e.IdApertura).HasColumnName("Id_Apertura");
            entity.Property(e => e.EstadoApertura)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Estado_Apertura");
            entity.Property(e => e.FechaApertura).HasColumnName("Fecha_Apertura");
            entity.Property(e => e.HoraApertura).HasColumnName("Hora_Apertura");
            entity.Property(e => e.MontoApertura).HasColumnName("Monto_Apertura");
        });

        modelBuilder.Entity<ArqueoCaja>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdApertura, e.IdArqueoCaja }).HasName("PK__Arqueo c__6F38EB3DA1661E60");

            entity.ToTable("Arqueo caja");

            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.IdApertura).HasColumnName("Id_Apertura");
            entity.Property(e => e.IdArqueoCaja)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id_Arqueo_Caja");
            entity.Property(e => e.FaltanteCordoba).HasColumnName("Faltante_Cordoba");
            entity.Property(e => e.FaltanteDolar).HasColumnName("Faltante_Dolar");
            entity.Property(e => e.FechaArqueo)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Arqueo");
            entity.Property(e => e.SobranteCordoba).HasColumnName("Sobrante_Cordoba");
            entity.Property(e => e.SobranteDolar).HasColumnName("Sobrante_Dolar");
            entity.Property(e => e.TotalEfectivoCordoba).HasColumnName("Total_Efectivo_Cordoba");
            entity.Property(e => e.TotalEfectivoDolar).HasColumnName("Total_Efectivo_Dolar");

            entity.HasOne(d => d.IdAperturaNavigation).WithMany(p => p.ArqueoCajas)
                .HasForeignKey(d => d.IdApertura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Arqueo ca__Id_Ap__5DCAEF64");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ArqueoCajas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Arqueo ca__Id_us__5812160E");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CB9033493D6E2943");

            entity.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");
            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.CedulaCliente).HasName("PK__Cliente__6E21107A5A7DF268");

            entity.ToTable("Cliente");

            entity.Property(e => e.CedulaCliente)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Cedula_Cliente");
            entity.Property(e => e.ApellidoCliente)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Apellido_Cliente");
            entity.Property(e => e.ColillaInssCliente)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Colilla_INSS_Cliente");
            entity.Property(e => e.DireccionCliente)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Direccion_Cliente");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Nombre_Cliente");
            entity.Property(e => e.SujetoCredito).HasColumnName("Sujeto_Credito");
            entity.Property(e => e.TelefonoCliente)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Telefono_Cliente");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PK__Compra__661E0ED03276DA7F");

            entity.ToTable("Compra");

            entity.Property(e => e.IdCompra).HasColumnName("Id_Compra");
            entity.Property(e => e.CodigoRuc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Codigo_Ruc");
            entity.Property(e => e.FechaCompra)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Compra");
            entity.Property(e => e.NumeroFactura).HasColumnName("Numero_Factura");
            entity.Property(e => e.TotalCompra).HasColumnName("Total_Compra");

            entity.HasOne(d => d.CodigoRucNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.CodigoRuc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Compra__Codigo_R__3F115E1A");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => new { e.CodigoProducto, e.IdDetalleCompra, e.IdCompra }).HasName("PK__Detalle___BA9A77DD028DCE39");

            entity.ToTable("Detalle_Compra");

            entity.Property(e => e.CodigoProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Codigo_Producto");
            entity.Property(e => e.IdDetalleCompra)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id_Detalle_Compra");
            entity.Property(e => e.IdCompra).HasColumnName("Id_Compra");
            entity.Property(e => e.CantidadCompra).HasColumnName("Cantidad_compra");
            entity.Property(e => e.PrecioCompra).HasColumnName("Precio_Compra");
            entity.Property(e => e.SubtotalCompra).HasColumnName("Subtotal_Compra");

            entity.HasOne(d => d.CodigoProductoNavigation).WithMany(p => p.DetalleCompras)
                .HasPrincipalKey(p => p.CodigoProducto)
                .HasForeignKey(d => d.CodigoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Detalle_C__Codig__59FA5E80");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Id_Compra");
        });

        modelBuilder.Entity<DetalleCredito>(entity =>
        {
            entity.HasKey(e => e.IdDetalleCredito).HasName("PK__Detalle___E6A889B98844EED6");

            entity.ToTable("Detalle_Credito");

            entity.Property(e => e.IdDetalleCredito).HasColumnName("Id_DetalleCredito");
            entity.Property(e => e.AbonoCapital).HasColumnName("Abono_Capital");
            entity.Property(e => e.CambioDevuelto).HasColumnName("Cambio_Devuelto");
            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Pago");
            entity.Property(e => e.IdCredito).HasColumnName("Id_Credito");
            entity.Property(e => e.InteresPagado).HasColumnName("Interes_Pagado");
            entity.Property(e => e.NumeroCuota).HasColumnName("Numero_Cuota");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalCordobas).HasColumnName("Total_Cordobas");
            entity.Property(e => e.TotalDolares).HasColumnName("Total_Dolares");
            entity.Property(e => e.UsuarioRegistro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Usuario_Registro");
            entity.Property(e => e.ValorCuota).HasColumnName("Valor_Cuota");

            entity.HasOne(d => d.IdCreditoNavigation).WithMany(p => p.DetalleCreditos)
                .HasForeignKey(d => d.IdCredito)
                .HasConstraintName("FK_DetalleCredito_Credito");
        });

        modelBuilder.Entity<DetalleDeVentum>(entity =>
        {
            entity.HasKey(e => new { e.IdDetalleVenta, e.IdVenta, e.CodigoProducto, e.CedulaCliente }).HasName("PK__Detalle___9D790501C7635FAD");

            entity.ToTable("Detalle_De_Venta");

            entity.Property(e => e.IdDetalleVenta)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id_Detalle_Venta");
            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.CodigoProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Codigo_Producto");
            entity.Property(e => e.CedulaCliente)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Cedula_Cliente");
            entity.Property(e => e.Precio)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SubTotal).HasColumnName("Sub_Total");

            entity.HasOne(d => d.CodigoProductoNavigation).WithMany(p => p.DetalleDeVenta)
                .HasPrincipalKey(p => p.CodigoProducto)
                .HasForeignKey(d => d.CodigoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Detalle_D__Codig__6383C8BA");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleDeVenta)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Detalle_D__Id_Ve__6477ECF3");
        });

        modelBuilder.Entity<Devolucion>(entity =>
        {
            entity.HasKey(e => new { e.IdDevolucion, e.IdVenta }).HasName("PK__Devoluci__6E35A86E61A7C310");

            entity.ToTable("Devolucion");

            entity.Property(e => e.IdDevolucion)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id_Devolucion");
            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.CantidadDevuelta).HasColumnName("Cantidad_Devuelta");
            entity.Property(e => e.CedulaCliente)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Cedula_Cliente");
            entity.Property(e => e.DescripcionDevolucion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Descripcion_Devolucion");
            entity.Property(e => e.FechaDevolucion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Fecha_Devolucion");
            entity.Property(e => e.MontoDevolucion).HasColumnName("Monto_Devolucion");
            entity.Property(e => e.MotivoDevolucion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Motivo_Devolucion");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.Devolucions)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Devolucio__Id_Ve__656C112C");
        });

        modelBuilder.Entity<Egreso>(entity =>
        {
            entity.HasKey(e => new { e.IdEgreso, e.IdUsuario, e.IdApertura, e.IdArqueoCaja }).HasName("PK__Egreso__BA278E38C622B37D");

            entity.ToTable("Egreso");

            entity.Property(e => e.IdEgreso)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id_Egreso");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.IdApertura).HasColumnName("Id_Apertura");
            entity.Property(e => e.IdArqueoCaja).HasColumnName("Id_Arqueo_Caja");
            entity.Property(e => e.CantidadEgresadaCordoba)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Cantidad_Egresada_Cordoba");
            entity.Property(e => e.CantidadEgresadaDolar)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Cantidad_Egresada_Dolar");
            entity.Property(e => e.FechaEgreso).HasColumnName("Fecha_Egreso");
            entity.Property(e => e.MotivoEgreso)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Motivo_Egreso");

            entity.HasOne(d => d.ArqueoCaja).WithMany(p => p.Egresos)
                .HasForeignKey(d => new { d.IdUsuario, d.IdApertura, d.IdArqueoCaja })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Egreso__5EBF139D");
        });

        modelBuilder.Entity<FacturaCredito>(entity =>
        {
            entity.HasKey(e => e.IdCredito).HasName("PK__Factura___9AA34D3F7D073964");

            entity.ToTable("Factura_Credito");

            entity.Property(e => e.IdCredito).HasColumnName("Id_Credito");
            entity.Property(e => e.EstadoCredito)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Estado_Credito");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Creacion");
            entity.Property(e => e.FechaFinal).HasColumnName("Fecha_Final");
            entity.Property(e => e.FechaInicio).HasColumnName("Fecha_Inicio");
            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.InteresMensual).HasColumnName("Interes_Mensual");
            entity.Property(e => e.MontoCredito).HasColumnName("Monto_Credito");
            entity.Property(e => e.NuevoSaldo).HasColumnName("Nuevo_Saldo");
            entity.Property(e => e.Observaciones).HasColumnType("text");
            entity.Property(e => e.PlazosMeses).HasColumnName("Plazos_Meses");
            entity.Property(e => e.TotalAbonado).HasColumnName("Total_Abonado");
            entity.Property(e => e.UsuarioRegistro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Usuario_Registro");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.FacturaCreditos)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK_Credito_Venta");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.IdMarca).HasName("PK__Marca__28EFE28A257303B6");

            entity.ToTable("Marca");

            entity.Property(e => e.IdMarca).HasColumnName("Id_Marca");
            entity.Property(e => e.Marca1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Marca");
        });

        modelBuilder.Entity<OtrasSalidasDeInventario>(entity =>
        {
            entity.HasKey(e => new { e.IdInventario, e.CodigoProducto }).HasName("PK__Otras_sa__39BB98027B17D20D");

            entity.ToTable("Otras_salidas_de_inventario");

            entity.Property(e => e.IdInventario)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id_Inventario");
            entity.Property(e => e.CodigoProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Codigo_Producto");
            entity.Property(e => e.CantidadSalir).HasColumnName("Cantidad_Salir");
            entity.Property(e => e.DescripcionSalida)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Descripcion_Salida");
            entity.Property(e => e.MotivoSalida)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Motivo_Salida");

            entity.HasOne(d => d.CodigoProductoNavigation).WithMany(p => p.OtrasSalidasDeInventarios)
                .HasPrincipalKey(p => p.CodigoProducto)
                .HasForeignKey(d => d.CodigoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Otras_sal__Codig__68487DD7");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => new { e.IdCategoria, e.IdMarca, e.CodigoProducto }).HasName("PK__Producto__9018C0D80DB5BE7D");

            entity.ToTable("Producto");

            entity.HasIndex(e => e.CodigoProducto, "UQ_Codigo_Producto").IsUnique();

            entity.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");
            entity.Property(e => e.IdMarca).HasColumnName("Id_Marca");
            entity.Property(e => e.CodigoProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Codigo_Producto");
            entity.Property(e => e.EstadoProducto).HasColumnName("Estado_Producto");
            entity.Property(e => e.ModeloProducto)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Modelo_Producto");
            entity.Property(e => e.PrecioVenta).HasColumnName("Precio_Venta");
            entity.Property(e => e.StockActual).HasColumnName("Stock_Actual");
            entity.Property(e => e.StockMinimo).HasColumnName("Stock_Minimo");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Producto__Id_Cat__693CA210");

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Producto__Id_Mar__6A30C649");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.CodigoRuc).HasName("PK__Proveedo__DB734E32E424E9EA");

            entity.ToTable("Proveedor");

            entity.Property(e => e.CodigoRuc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Codigo_Ruc");
            entity.Property(e => e.ApellidoProveedor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Apellido_Proveedor");
            entity.Property(e => e.CorreoProveedor)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Correo_Proveedor");
            entity.Property(e => e.EstadoProveedor).HasColumnName("Estado_Proveedor");
            entity.Property(e => e.NombreProveedor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Nombre_Proveedor");
            entity.Property(e => e.TelefonoProveedor)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Telefono_Proveedor");
        });

        modelBuilder.Entity<TasaDeCambio>(entity =>
        {
            entity.HasKey(e => e.IdTasaCambio).HasName("PK__Tasa de __D4136D075360949B");

            entity.ToTable("Tasa de cambio");

            entity.Property(e => e.IdTasaCambio).HasColumnName("Id_Tasa_Cambio");
            entity.Property(e => e.FechaCambio).HasColumnName("Fecha_Cambio");
            entity.Property(e => e.ValorCambio).HasColumnName("Valor_Cambio");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__EF59F76236357D9F");

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.ApellidoUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Usuario");
            entity.Property(e => e.ContraseñaUsuario)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Contraseña_Usuario");
            entity.Property(e => e.CorreoUsuario)
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Correo_Usuario");
            entity.Property(e => e.EstadoUsuario).HasColumnName("Estado_Usuario");
            entity.Property(e => e.FechaRecuperacion).HasColumnName("Fecha_Recuperacion");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Nombre_Usuario");
            entity.Property(e => e.RolUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Rol_Usuario");
            entity.Property(e => e.TelefonoUsuario)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Telefono_Usuario");
            entity.Property(e => e.TokenRecuperacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Token_Recuperacion");
            entity.Property(e => e.UsuarioLogueo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Usuario_Logueo");
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__Venta__B3C9EABD41B48BA5");

            entity.Property(e => e.IdVenta)
                .ValueGeneratedNever()
                .HasColumnName("Id_Venta");
            entity.Property(e => e.CambioVenta).HasColumnName("Cambio_Venta");
            entity.Property(e => e.CedulaCliente)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Cedula_Cliente");
            entity.Property(e => e.FechaVenta).HasColumnName("Fecha_Venta");
            entity.Property(e => e.PagoCordobas).HasColumnName("Pago_Cordobas");
            entity.Property(e => e.PagoDolares).HasColumnName("Pago_Dolares");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Tipo_Pago");
            entity.Property(e => e.TotalVenta).HasColumnName("Total_Venta");

            entity.HasOne(d => d.CedulaClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.CedulaCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Venta__Cedula_Cl__6B24EA82");
        });

        modelBuilder.Entity<VistaArqueoCajaPorPeriodoCajero>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vista_Arqueo_Caja_Por_Periodo_Cajero");

            entity.Property(e => e.Cajero)
                .HasMaxLength(101)
                .IsUnicode(false);
            entity.Property(e => e.FaltanteCordoba).HasColumnName("Faltante_Cordoba");
            entity.Property(e => e.FaltanteDolar).HasColumnName("Faltante_Dolar");
            entity.Property(e => e.FechaApertura).HasColumnName("Fecha_Apertura");
            entity.Property(e => e.FechaArqueo)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Arqueo");
            entity.Property(e => e.HoraApertura).HasColumnName("Hora_Apertura");
            entity.Property(e => e.IdApertura).HasColumnName("Id_Apertura");
            entity.Property(e => e.IdArqueoCaja).HasColumnName("Id_Arqueo_Caja");
            entity.Property(e => e.MontoApertura).HasColumnName("Monto_Apertura");
            entity.Property(e => e.SobranteCordoba).HasColumnName("Sobrante_Cordoba");
            entity.Property(e => e.SobranteDolar).HasColumnName("Sobrante_Dolar");
            entity.Property(e => e.TotalEfectivoCordoba).HasColumnName("Total_Efectivo_Cordoba");
            entity.Property(e => e.TotalEfectivoDolar).HasColumnName("Total_Efectivo_Dolar");
            entity.Property(e => e.TotalVentasCordobas).HasColumnName("Total_Ventas_Cordobas");
            entity.Property(e => e.TotalVentasDolares).HasColumnName("Total_Ventas_Dolares");
        });

        modelBuilder.Entity<VistaInventarioActual>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vista_Inventario_Actual");

            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CodigoProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Codigo_Producto");
            entity.Property(e => e.EstadoStock)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Estado_Stock");
            entity.Property(e => e.Marca)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrecioVenta).HasColumnName("Precio_Venta");
            entity.Property(e => e.Producto)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.StockActual).HasColumnName("Stock_Actual");
            entity.Property(e => e.StockMinimo).HasColumnName("Stock_Minimo");
            entity.Property(e => e.ValorTotalInventario).HasColumnName("Valor_Total_Inventario");
        });

        modelBuilder.Entity<VistaSalidasInventarioPorPeriodoMotivo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vista_Salidas_Inventario_Por_Periodo_Motivo");

            entity.Property(e => e.CantidadSalir).HasColumnName("Cantidad_Salir");
            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CodigoProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Codigo_Producto");
            entity.Property(e => e.DescripcionSalida)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Descripcion_Salida");
            entity.Property(e => e.FechaSalida)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Salida");
            entity.Property(e => e.IdInventario).HasColumnName("Id_Inventario");
            entity.Property(e => e.Marca)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MotivoSalida)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Motivo_Salida");
            entity.Property(e => e.Producto)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VistaStockProximoAgotarse>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vista_Stock_Proximo_Agotarse");

            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CodigoProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Codigo_Producto");
            entity.Property(e => e.EstadoReabastecimiento)
                .HasMaxLength(33)
                .IsUnicode(false)
                .HasColumnName("Estado_Reabastecimiento");
            entity.Property(e => e.Marca)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrecioVenta).HasColumnName("Precio_Venta");
            entity.Property(e => e.Producto)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.StockActual).HasColumnName("Stock_Actual");
            entity.Property(e => e.StockMinimo).HasColumnName("Stock_Minimo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
