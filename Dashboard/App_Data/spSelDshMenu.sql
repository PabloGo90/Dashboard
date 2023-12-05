CREATE PROCEDURE [dbo].[spSelDshMenu] AS
BEGIN

	select id as Id,
		nombreMenu as Nombre,
		nombreIconoMenu as Icono,
		orden as Orden,
		nombreAgrupadorMenu as Parent
	from DshModulos (nolock)
	where activo = 'Y'
	order by orden
END