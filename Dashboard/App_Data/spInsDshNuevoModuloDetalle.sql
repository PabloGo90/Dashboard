CREATE PROCEDURE [dbo].[spInsDshNuevoModuloDetalle] 
	@id int,
	@nombreGrafico varchar(100),
	@nombreCorto varchar(100),
	@tipoGrafico varchar(100)
	AS
BEGIN
	INSERT INTO DshModulosDetalle (idModulo, nombreGrafico, nombreCorto, tipoGrafico)
	SELECT  @id, @nombreGrafico, @nombreCorto,@tipoGrafico

END