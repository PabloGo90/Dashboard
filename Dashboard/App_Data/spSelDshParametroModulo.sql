CREATE PROCEDURE [dbo].[spSelDshParametroModulo] 
	@id Int
AS BEGIN

	SELECT idModulo, 
			nombreParSP, 
			nombreParFiltro, 
			valorDefecto, 
			obligatorio, 
			tipo, 
			tipoHtml, 
			selectListSPData, 
			orden, 
            largoMax, 
			validacion, 
			validacionCondicion, 
			validacionCondicionValor, 
			largoMin, 
			valorMin, 
			valorMax
	FROM DshParametros
	Where idModulo = @id;
END
        