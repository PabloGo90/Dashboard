CREATE PROCEDURE [dbo].[spSelDshModuloInfo]
	@id int
AS
BEGIN

	--info
	select id,
		nombre,
		descripcion,
		nombreMenu,
		nombreIconoMenu,
		nombreAgrupadorMenu,
		orden,
		activo
	from DshModulos (nolock)
	where id = @id
	order by nombreAgrupadorMenu, orden

	--items
	select id,	
		idModulo,	
		nombreGrafico,	
		nombreCorto,
		tipoGrafico
	from DshModulosDetalle 
	where idModulo = @id
	
	--dataset
	select mds.id,	
		md.idModulo,
		mds.idModuloDet,	
		mds.dsTitulo,	
		mds.nombreSP,	
		mds.getLabelFromColumn,
		mds.getDataFromColumn,
		mds.dataOperation
	from DshModulosDetalle md 
	inner join DshModulosDetalleDataset mds on mds.idModuloDet = md.id
	where md.idModulo = @id

	--parametros
	select id ,
		idModulo,
		nombreParSP	,
		nombreParFiltro	,
		valorDefecto,
		obligatorio	,
		tipo		,
		tipoHtml	,
		selectListSPData,
		largoMin,
		largoMax,
		valorMin,
		valorMax,	
		validacion	,
		validacionCondicion	,
		validacionCondicionValor,
		orden
	from DshParametros (nolock)
	where idModulo = @id
	order by orden
END