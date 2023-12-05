CREATE PROCEDURE [dbo].[spSelDshModuloDetalle] 
	@id int
	AS
BEGIN
	SELECT  id
			,idModulo
			,nombreGrafico
			,nombreCorto
			,tipoGrafico
	FROM DshModulosDetalle 
	WHERE idModulo = @id

END