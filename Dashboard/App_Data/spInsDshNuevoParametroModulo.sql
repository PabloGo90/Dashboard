CREATE PROCEDURE [dbo].[spInsDshNuevoParametroModulo] 
	@id Int,
	@nombreParSP varchar(100),
	@nombreParFiltro varchar(100),
	@valorDefecto varchar(100),
	@obligatorio Int,
	@tipo varchar(100),
	@tipoHtml varchar(100),
	@selectListSPData varchar(100),
	@orden Int,
	@largoMax Int,
	@validacion varchar(100),
	@validacionCondicion varchar(100),
	@validacionCondicionValor varchar(100),
	@largoMin Int,
	@valorMin varchar(100),
	@valorMax varchar(100)
	AS
BEGIN
INSERT INTO
	DshParametros(idModulo, nombreParSP, nombreParFiltro, valorDefecto, obligatorio, tipo, tipoHtml, selectListSPData, orden, 
                  largoMax, validacion, validacionCondicion, validacionCondicionValor, largoMin, valorMin, valorMax)
	select @id, @nombreParSP, @nombreParFiltro, @valorDefecto, @obligatorio, @tipo,@tipoHtml, @selectListSPData,@orden, 
           @largoMax, @validacion, @validacionCondicion, @validacionCondicionValor, @largoMin, @valorMin, @valorMax;

END
        