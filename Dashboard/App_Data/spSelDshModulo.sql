CREATE PROCEDURE [dbo].[spSelDshModulo] 
	@id int
	AS
BEGIN
	
	SELECT nombreAgrupadorMenu,nombre,descripcion, nombreMenu, nombreIconoMenu, activo, orden
	FROM DshModulos
	WHERE id = @id

END