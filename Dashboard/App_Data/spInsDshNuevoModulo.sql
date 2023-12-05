CREATE PROCEDURE [dbo].[spInsDshNuevoModulo] 
	@nombreAgrupadorMenu varchar(100),
	@nombre varchar(100),
	@descripcion varchar(max),
	@nombreMenu varchar(100),
	@nombreIconoMenu varchar(100),
	@activo varchar(1),
	@orden int
	AS
BEGIN
	INSERT INTO  DshModulos(nombreAgrupadorMenu,nombre,descripcion, nombreMenu, nombreIconoMenu, activo, orden) 
    select @nombreAgrupadorMenu,@nombre,@descripcion, @nombreMenu, @nombreIconoMenu, @activo, @orden;
END