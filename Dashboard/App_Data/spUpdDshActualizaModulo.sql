CREATE PROCEDURE [dbo].[spUpdDshActualizaModulo] 
	@id Int,
	@nombre varchar(100),
	@descripcion varchar(100),
	@nombreMenu varchar(100),
	@nombreIconoMenu varchar(100),
	@nombreAgrupadorMenu varchar(100),
	@orden int,
	@activo varchar(1)
	AS
BEGIN
	update DshModulos 
	set nombre = @nombre, 
	    descripcion = @descripcion, 
	    nombreMenu = @nombreMenu, 
	    nombreIconoMenu = @nombreIconoMenu, 
		nombreAgrupadorMenu = @nombreAgrupadorMenu,
	    orden = @orden, 
	    activo = @activo
	where id= @id;
END