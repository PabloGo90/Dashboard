CREATE PROCEDURE [dbo].[spSelDshIDModuloDetalle] @idModulo INT, @nombreDetalle VARCHAR(100) AS
BEGIN
	SELECT top 1 id
	FROM DshModulosDetalle
	WHERE idModulo= @idModulo AND nombreGrafico = @nombreDetalle;
END