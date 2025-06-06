USE [master]
GO
/****** Object:  Database [MotosTuning]    Script Date: 4/29/2025 9:06:58 AM ******/
CREATE DATABASE [MotosTuning]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MotosTuning', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\MotosTuning.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MotosTuning_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\MotosTuning_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [MotosTuning] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MotosTuning].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MotosTuning] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MotosTuning] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MotosTuning] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MotosTuning] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MotosTuning] SET ARITHABORT OFF 
GO
ALTER DATABASE [MotosTuning] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [MotosTuning] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MotosTuning] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MotosTuning] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MotosTuning] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MotosTuning] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MotosTuning] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MotosTuning] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MotosTuning] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MotosTuning] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MotosTuning] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MotosTuning] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MotosTuning] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MotosTuning] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MotosTuning] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MotosTuning] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MotosTuning] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MotosTuning] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MotosTuning] SET  MULTI_USER 
GO
ALTER DATABASE [MotosTuning] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MotosTuning] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MotosTuning] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MotosTuning] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MotosTuning] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MotosTuning] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [MotosTuning] SET QUERY_STORE = ON
GO
ALTER DATABASE [MotosTuning] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [MotosTuning]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Citas]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Citas](
	[IdCita] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[FechaCita] [datetime] NOT NULL,
	[Estado] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClienteMoto]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClienteMoto](
	[IdCliente] [int] NOT NULL,
	[IdMoto] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC,
	[IdMoto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[IdCliente] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Telefono] [varchar](15) NOT NULL,
	[Correo] [varchar](100) NULL,
	[Direccion] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompraPiezas]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompraPiezas](
	[IdCompra] [int] NOT NULL,
	[IdPieza] [int] NOT NULL,
	[CantidadComprada] [int] NOT NULL,
	[PrecioUnitario] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCompra] ASC,
	[IdPieza] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Compras]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compras](
	[IdCompra] [int] IDENTITY(1,1) NOT NULL,
	[IdProveedor] [int] NOT NULL,
	[FechaCompra] [datetime] NULL,
	[MontoTotal] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Detalle_Reparacion]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Detalle_Reparacion](
	[Id_DetalleRep] [int] NOT NULL,
	[IdReparaciones] [int] NULL,
	[Idservicios] [int] NULL,
	[CostoServicio] [decimal](10, 2) NULL,
 CONSTRAINT [PK_Detalle_Reparacion] PRIMARY KEY CLUSTERED 
(
	[Id_DetalleRep] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Facturas]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Facturas](
	[IdFactura] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[IdReparacion] [int] NOT NULL,
	[FechaFactura] [datetime] NULL,
	[MontoTotal] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdFactura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inventario]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventario](
	[IdPieza] [int] IDENTITY(1,1) NOT NULL,
	[NombrePieza] [varchar](100) NOT NULL,
	[Cantidad] [int] NOT NULL,
	[Precio] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPieza] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Motos]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Motos](
	[IdMoto] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NULL,
	[Placa] [varchar](10) NOT NULL,
	[Marca] [varchar](50) NOT NULL,
	[Modelo] [varchar](50) NOT NULL,
	[Año] [int] NOT NULL,
	[Color] [varchar](20) NOT NULL,
	[EstadoEntrada] [text] NOT NULL,
	[EstadoSalida] [text] NULL,
 CONSTRAINT [PK__Motos__33CED5FBB9179DC7] PRIMARY KEY CLUSTERED 
(
	[IdMoto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Motos__8310F99D9BBBAF32] UNIQUE NONCLUSTERED 
(
	[Placa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pagos]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pagos](
	[IdPago] [int] IDENTITY(1,1) NOT NULL,
	[IdFactura] [int] NOT NULL,
	[MetodoPago] [varchar](50) NOT NULL,
	[Monto] [decimal](10, 2) NOT NULL,
	[FechaPago] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proveedores]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedores](
	[IdProveedor] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Telefono] [varchar](15) NOT NULL,
	[Correo] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdProveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reparaciones]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reparaciones](
	[IdReparacion] [int] IDENTITY(1,1) NOT NULL,
	[IdMoto] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Descripcion] [text] NOT NULL,
	[Estado] [varchar](50) NULL,
	[FechaInicio] [datetime] NULL,
	[FechaFin] [datetime] NULL,
	[Total] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdReparacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReparacionPiezas]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReparacionPiezas](
	[IdReparacion] [int] NOT NULL,
	[IdPieza] [int] NOT NULL,
	[CantidadUsada] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdReparacion] ASC,
	[IdPieza] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[NombreRol] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Servicios]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servicios](
	[IdServicio] [int] NOT NULL,
	[Servicio] [varchar](50) NULL,
	[Precio_Servicio] [decimal](10, 2) NULL,
 CONSTRAINT [PK_Servicios] PRIMARY KEY CLUSTERED 
(
	[IdServicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 4/29/2025 9:06:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Apellido] [varchar](50) NULL,
	[Telefono] [int] NULL,
	[Correo] [varchar](100) NOT NULL,
	[Usuario] [varchar](50) NULL,
	[Contraseña] [varchar](255) NOT NULL,
	[IdRol] [int] NOT NULL,
 CONSTRAINT [PK__Usuarios__5B65BF9777808F0E] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Usuarios__60695A19C7A930F1] UNIQUE NONCLUSTERED 
(
	[Correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Citas] ADD  DEFAULT ('Pendiente') FOR [Estado]
GO
ALTER TABLE [dbo].[Compras] ADD  DEFAULT (getdate()) FOR [FechaCompra]
GO
ALTER TABLE [dbo].[Facturas] ADD  DEFAULT (getdate()) FOR [FechaFactura]
GO
ALTER TABLE [dbo].[Pagos] ADD  DEFAULT (getdate()) FOR [FechaPago]
GO
ALTER TABLE [dbo].[Reparaciones] ADD  DEFAULT ('En proceso') FOR [Estado]
GO
ALTER TABLE [dbo].[Reparaciones] ADD  DEFAULT (getdate()) FOR [FechaInicio]
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD  CONSTRAINT [FK_Citas_Clientes] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[Citas] CHECK CONSTRAINT [FK_Citas_Clientes]
GO
ALTER TABLE [dbo].[ClienteMoto]  WITH CHECK ADD FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[ClienteMoto]  WITH CHECK ADD  CONSTRAINT [FK__ClienteMo__IdMot__44FF419A] FOREIGN KEY([IdMoto])
REFERENCES [dbo].[Motos] ([IdMoto])
GO
ALTER TABLE [dbo].[ClienteMoto] CHECK CONSTRAINT [FK__ClienteMo__IdMot__44FF419A]
GO
ALTER TABLE [dbo].[CompraPiezas]  WITH CHECK ADD FOREIGN KEY([IdCompra])
REFERENCES [dbo].[Compras] ([IdCompra])
GO
ALTER TABLE [dbo].[CompraPiezas]  WITH CHECK ADD FOREIGN KEY([IdPieza])
REFERENCES [dbo].[Inventario] ([IdPieza])
GO
ALTER TABLE [dbo].[Compras]  WITH CHECK ADD FOREIGN KEY([IdProveedor])
REFERENCES [dbo].[Proveedores] ([IdProveedor])
GO
ALTER TABLE [dbo].[Detalle_Reparacion]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_Reparacion_Reparaciones] FOREIGN KEY([IdReparaciones])
REFERENCES [dbo].[Reparaciones] ([IdReparacion])
GO
ALTER TABLE [dbo].[Detalle_Reparacion] CHECK CONSTRAINT [FK_Detalle_Reparacion_Reparaciones]
GO
ALTER TABLE [dbo].[Detalle_Reparacion]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_Reparacion_Servicios] FOREIGN KEY([Idservicios])
REFERENCES [dbo].[Servicios] ([IdServicio])
GO
ALTER TABLE [dbo].[Detalle_Reparacion] CHECK CONSTRAINT [FK_Detalle_Reparacion_Servicios]
GO
ALTER TABLE [dbo].[Facturas]  WITH CHECK ADD FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[Facturas]  WITH CHECK ADD FOREIGN KEY([IdReparacion])
REFERENCES [dbo].[Reparaciones] ([IdReparacion])
GO
ALTER TABLE [dbo].[Motos]  WITH CHECK ADD  CONSTRAINT [FK_Motos_Clientes] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[Motos] CHECK CONSTRAINT [FK_Motos_Clientes]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD FOREIGN KEY([IdFactura])
REFERENCES [dbo].[Facturas] ([IdFactura])
GO
ALTER TABLE [dbo].[Reparaciones]  WITH CHECK ADD  CONSTRAINT [FK__Reparacio__IdMot__4D94879B] FOREIGN KEY([IdMoto])
REFERENCES [dbo].[Motos] ([IdMoto])
GO
ALTER TABLE [dbo].[Reparaciones] CHECK CONSTRAINT [FK__Reparacio__IdMot__4D94879B]
GO
ALTER TABLE [dbo].[Reparaciones]  WITH CHECK ADD  CONSTRAINT [FK__Reparacio__IdUsu__4E88ABD4] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Reparaciones] CHECK CONSTRAINT [FK__Reparacio__IdUsu__4E88ABD4]
GO
ALTER TABLE [dbo].[ReparacionPiezas]  WITH CHECK ADD FOREIGN KEY([IdPieza])
REFERENCES [dbo].[Inventario] ([IdPieza])
GO
ALTER TABLE [dbo].[ReparacionPiezas]  WITH CHECK ADD FOREIGN KEY([IdReparacion])
REFERENCES [dbo].[Reparaciones] ([IdReparacion])
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK__Usuarios__IdRol__3A81B327] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Roles] ([IdRol])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK__Usuarios__IdRol__3A81B327]
GO
USE [master]
GO
ALTER DATABASE [MotosTuning] SET  READ_WRITE 
GO
