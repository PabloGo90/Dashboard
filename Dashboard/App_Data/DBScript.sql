CREATE TABLE DshModulos (
	id int NOT NULL identity (1,1),
	nombre NVARCHAR(100),
	descripcion NVARCHAR(max),
	nombreMenu NVARCHAR(50),
	nombreIconoMenu NVARCHAR(50),
	nombreAgrupadorMenu NVARCHAR(50),
	orden int,
	activo char(1),
	PRIMARY KEY (id)
)

CREATE TABLE DshModulosDetalle (
	id int NOT NULL identity (1,1),
    idModulo int FOREIGN KEY REFERENCES DshModulos(id),
	nombreGrafico NVARCHAR(100),
	nombreCorto NVARCHAR(50),
	tipoGrafico NVARCHAR(50),
	PRIMARY KEY (id)
)

CREATE TABLE DshModulosDetalleDataset (
	id int NOT NULL identity (1,1),
    idModuloDet int FOREIGN KEY REFERENCES DshModulosDetalle(id),
    idModulo int ,
	dsTitulo NVARCHAR(50),
	nombreSP NVARCHAR(100),
	getLabelFromColumn NVARCHAR(20),
	getDataFromColumn NVARCHAR(20),
	dataOperation NVARCHAR(10),
	PRIMARY KEY (id)
)

CREATE TABLE DshParametros (
	id int NOT NULL identity (1,1),
    idModulo int FOREIGN KEY REFERENCES DshModulos(id),
	nombreParSP NVARCHAR(50),
	nombreParFiltro NVARCHAR(50),
	valorDefecto NVARCHAR(50),
	obligatorio tinyint,
	tipo NVARCHAR(10),
	tipoHtml NVARCHAR(10),
	selectListSPData NVARCHAR(50),
	largoMin INTEGER,
	largoMax INTEGER,
	valorMin NVARCHAR(12),
	valorMax NVARCHAR(12),
	validacion NVARCHAR(12)	,
	validacionCondicion NVARCHAR(12),
	validacionCondicionValor NVARCHAR(20),
	orden int,
	PRIMARY KEY (id)
)

CREATE TABLE [dbo].[DshParametros_Sistema](
	[id_param] [int] IDENTITY(1,1) PRIMARY KEY  NOT NULL,
	[variable_param] [varchar](100) NOT NULL,
	[nombre_param] [varchar](100) NULL,
	[valor_param] [varchar](100) NOT NULL,
	[activo] [bit] NULL
)

CREATE TABLE [dbo].[DshHist_passw](
	[id_usuario] [int] NOT NULL,
	[fecha_cambio_clave] [datetime] NULL,
	[passwd_usuario] [varbinary](256) NULL
) 

CREATE TABLE [dbo].[DshUsuarios](
	[idUser] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[nombreCompleto] [nvarchar](300) NOT NULL,
	[pass] [varbinary](max) NOT NULL,
	[rut] [nvarchar](10) NULL,
	[activo] [bit] NULL,
	[loginUsuario] [varchar](20) NULL,
	[fechaUltimoIngreso] [datetime] NULL,
	[correo] [nvarchar](50) NULL,
	[fono] [nvarchar](50) NULL,
	[ip_usuario] [nvarchar](50) NULL,
	[caducidad] [int] NULL,
	[intentos_fallidos] [int] NULL,
	[FechaActivacion] [datetime] NULL,
	[FechaExpiracion] [datetime] NULL,
	[conectado] [bit] NULL,
	[isAdmin] [bit] NOT NULL
)

CREATE TABLE [dbo].[DshAuditoria](
	[id] [int] IDENTITY(1,1) PRIMARY KEY  NOT NULL,
	[idUser] [int] NULL,
	[idUserLogin] [nvarchar](20) NULL,
	[ip_usuario] [nvarchar](50) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[accion] [nvarchar](2) NOT NULL,
	[pagina] [nvarchar](50) NOT NULL,
	[descripcion] [nvarchar](50) NOT NULL
	) 



GO
INSERT INTO DshParametros_Sistema (variable_param, nombre_param, valor_param, activo)
values ('SymmKeyPwUsuario',	'Llave simétrica password usuario', 'P4ssw0rdUsu4r10', 1)
INSERT INTO DshParametros_Sistema (variable_param, nombre_param, valor_param, activo)
values ('DIASEXPIRACIONPWD',	'Días de expiración password',	30,	1)
INSERT INTO DshParametros_Sistema (variable_param, nombre_param, valor_param, activo)
values ('CANTIDADHISTORICOS',	'Cantidad de password guardadas en histórico',	4,	1)
INSERT INTO DshParametros_Sistema (variable_param, nombre_param, valor_param, activo)
values ('DIASINACTIVA',			'Días de inactividad',	30,	1)
INSERT INTO DshParametros_Sistema (variable_param, nombre_param, valor_param, activo)
values ('MAXINTENTOS',			'Intentos fallidos permitidos',	3	,1)