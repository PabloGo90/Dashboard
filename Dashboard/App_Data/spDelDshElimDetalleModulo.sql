CREATE PROCEDURE [dbo].[spDelDshElimDetalleModulo] 
	@id Int
	AS
BEGIN
	Delete from DshParametros where idModulo = @id;
	Delete from DshModulosDetalleDataset where idModulo = @id;
	Delete from DshModulosDetalle where idModulo = @id;
END