CREATE PROCEDURE [dbo].[spInsDshNuevoModuloDetalleDS] 
	@idMod int,
	@idDet int,
	@dsTitulo varchar(100),
	@nombreSP varchar(100),
	@getLabelFromColumn varchar(100),
	@getDataFromColumn varchar(100),
	@dataOperation varchar(100)
	AS
BEGIN
	INSERT INTO DshModulosDetalleDataset (idModulo, idModuloDet, dsTitulo, nombreSP, getLabelFromColumn, getDataFromColumn, dataOperation)
	select @idMod, @idDet,@dsTitulo, @nombreSP, @getLabelFromColumn, @getDataFromColumn,@dataOperation

END