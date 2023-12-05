CREATE PROCEDURE [dbo].[spSelDshHome] AS
BEGIN

	--info
	select m.id as Id,
		m.nombreMenu as Nombre,
		m.descripcion as Descripcion,
		(select top 1 tipoGrafico from DshModulosDetalle where m.id = idModulo order by id)  as TipoGraficoStr,
		m.orden as Orden
	from DshModulos (nolock) m
	where m.activo = 'Y'
END