CREATE PROCEDURE [dbo].[spSelDshModuloDetalleDS] 
	@idDet int
	AS
BEGIN
	SELECT dsTitulo, nombreSP, getLabelFromColumn, getDataFromColumn, dataOperation
	FROM DshModulosDetalleDataset  
	WHERE id = @idDet
END