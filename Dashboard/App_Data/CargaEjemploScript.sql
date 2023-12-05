/*
select * from DshModulos
select * from DshModulosDetalle
select * from DshModulosDetalleDataset
select * from DshParametros
*/

insert into DshModulos(nombre, nombreMenu, nombreIconoMenu, activo, orden)
select 'Dashboard ejemplo1','Ejemplo 1','file-earmark', 'Y', 1 union all
select 'Ejemplo 2 info','Ejemplo 2','people', 'Y', 2 union all
select 'Modulo ejemplo 3','Ejemplo 3','file-earmark', 'Y', 3 union all
select 'Dashboard ejemplo4','Ejemplo 4','puzzle', 'Y', 4 union all
select 'Ejemplo 5 mod','Ejemplo 5','umbrella', 'Y', 5 

Insert into DshModulosDetalle (idModulo, nombreGrafico, nombreCorto, tipoGrafico)
select 1, 'grafico barra ej1', 'grafico 1', 'pie' union all
select 1, 'grafico 2 ej1', 'grafico 2', 'line' union all
select 2, 'grafico barra ej1', 'grafico 1', 'bar' union all
select 2, 'grafico linea ej2', 'grafico 2', 'line' union all
select 3, 'grafico barra ej3', 'grafico 1', 'bar' union all
select 3, 'grafico linea ej3', 'grafico 2', 'line' union all
select 3, 'grafico barra ej3', 'grafico 3', 'bar' union all
select 4, 'grafico barra ej4', 'grafico 1', 'bar' union all
select 5, 'grafico barra ej5', 'grafico 1', 'bar' union all
select 5, 'grafico linea ej5', 'grafico 2', 'line'

Insert into DshModulosDetalleDataset(idModuloDet, dsTitulo, nombreSP, getLabelFromColumn, getDataFromColumn)
select 1, 'Actividad 1', 'spGetDshDataEjemplo1',	'colLabel', 'colData' union all
select 2, 'Actividad 1', 'spGetDshDataEjemplo1',	'colLabel', 'colData' union all
select 2, 'Actividad 2', 'spGetDshDataEjemplo2',	'colLabel', 'colData' union all
select 2, 'Actividad 3', 'spGetDshDataEjemplo3',	'colLabel', 'colData' union all
select 3, 'Actividad 1', 'spGetDshDataEjemplo1',	'colLabel', 'colData' union all
select 3, 'Actividad 2', 'spGetDshDataEjemplo2',	'colLabel', 'colData' union all
select 4, 'Actividad 1', 'spGetDshDataEjemplo1',	'colLabel', 'colData' union all
select 4, 'Actividad 2', 'spGetDshDataEjemplo2',	'colLabel', 'colData' union all
select 5, 'Actividad 1', 'spGetDshDataEjemplo1',	'colLabel', 'colData' union all
select 5, 'Actividad 2', 'spGetDshDataEjemplo2',	'colLabel', 'colData' union all
select 6, 'Actividad 1', 'spGetDshDataEjemplo1',	'colLabel', 'colData' union all
select 6, 'Actividad 2', 'spGetDshDataEjemplo2',	'colLabel', 'colData' union all
select 7, 'Actividad 1', 'spGetDshDataEjemplo1',	'colLabel', 'colData' union all
select 7, 'Actividad 2', 'spGetDshDataEjemplo2',	'colLabel', 'colData' union all
select 8, 'Actividad 1', 'spGetDshDataEjemplo1',	'colLabel', 'colData' union all
select 8, 'Actividad 2', 'spGetDshDataEjemplo2',	'colLabel', 'colData' union all
select 9, 'Actividad 1', 'spGetDshDataEjemplo1',	'colLabel', 'colData' union all
select 9, 'Actividad 2', 'spGetDshDataEjemplo2',	'colLabel', 'colData' union all
select 10, 'Actividad 1', 'spGetDshDataEjemplo1',	'colLabel', 'colData' union all
select 10, 'Actividad 2', 'spGetDshDataEjemplo2',	'colLabel', 'colData' 

insert into DshParametros(idModulo, nombreParSP, nombreParFiltro, valorDefecto, obligatorio, tipo, orden)
select 1, 'usuario', 'Usuario', '', 1, 'string',1 union all
select 1, 'fechaDesde', 'fecha Desde', 'now', 1, 'date',2 union all
select 1, 'fechaHasta', 'fecha Hasta', 'now', 1, 'date',3 union all
select 2, 'usuario', 'Usuario', '', 1, 'string',1 union all
select 2, 'fechaDesde', 'fecha Desde', 'now', 1, 'date',2 union all
select 2, 'fechaHasta', 'fecha Hasta', 'now', 1, 'date',3 union all
select 3, 'usuario', 'Usuario', '', 1, 'string',1 union all
select 3, 'fechaDesde', 'fecha Desde', 'now', 1, 'date',2 union all
select 3, 'fechaHasta', 'fecha Hasta', 'now', 1, 'date',3 union all
select 4, 'usuario', 'Usuario', '', 1, 'string',1 union all
select 4, 'fechaDesde', 'fecha Desde', 'now', 1, 'date',2 union all
select 4, 'fechaHasta', 'fecha Hasta', 'now', 1, 'date',3 union all
select 5, 'usuario', 'Usuario', '', 1, 'string',1 union all
select 5, 'fechaDesde', 'fecha Desde', 'now', 1, 'date',2 union all
select 5, 'fechaHasta', 'fecha Hasta', 'now', 1, 'date',3 

go
CREATE PROCEDURE [dbo].[spGetDshDataEjemplo1] 	@usuario varchar(30), @fechaDesde varchar(10), @fechaHasta varchar(10) AS 
 BEGIN
	select u.nombreCompleto as colLabel, a.Nombre_Actividad, count(1) as colData
	from SeguimientoProductos p
	inner join usuarios u on u.idUser = p.idUser
	inner join actividades a on a.IdActividad = p.idActividad
	where convert(varchar(10),fechaActividad  ,112) between @fechaDesde and @fechaHasta
	and a.Nombre_Actividad = 'Recepción de Operaciones'
	group by u.nombreCompleto, a.Nombre_Actividad
END

go
CREATE PROCEDURE [dbo].[spGetDshDataEjemplo2] 	@usuario varchar(30), @fechaDesde varchar(10), @fechaHasta varchar(10) AS 
 BEGIN
	select u.nombreCompleto as colLabel, a.Nombre_Actividad, count(1) as colData
	from SeguimientoProductos p
	inner join usuarios u on u.idUser = p.idUser
	inner join actividades a on a.IdActividad = p.idActividad
	where convert(varchar(10),fechaActividad  ,112) between @fechaDesde and @fechaHasta
	and a.Nombre_Actividad <> 'Recepción de Operaciones'
	group by u.nombreCompleto, a.Nombre_Actividad
END

go
CREATE PROCEDURE [dbo].[spGetDshDataEjemplo3] 	@usuario varchar(30), @fechaDesde varchar(10), @fechaHasta varchar(10) AS 
 BEGIN
	select u.nombreCompleto as colLabel, a.Nombre_Actividad,  AVG(idSucursal) as colData
	from SeguimientoProductos p
	inner join usuarios u on u.idUser = p.idUser
	inner join actividades a on a.IdActividad = p.idActividad
	where convert(varchar(10),fechaActividad  ,112) between @fechaDesde and @fechaHasta
	and a.Nombre_Actividad <> 'Recepción de Operaciones'
	group by u.nombreCompleto, a.Nombre_Actividad
END

go
CREATE PROCEDURE [dbo].[spGetDshDataComboBox]  AS 
 BEGIN
	select 1 as value	, 'user1' as nameNivel1, 'area1' as nameNivel2, 'macroarea1' as nameNivel3
	union all select 2	, 'user2',	'area1','macroarea1'
	union all select 3	, 'user3',	'area1','macroarea1'
	union all select 4	, 'user4',	'area1','macroarea1'
	union all select 5	, 'user5',	'area1','macroarea1'
	union all select 6	, 'user6',	'area1','macroarea1'
	union all select 7	, 'user7',	'area2','macroarea1'
	union all select 8	, 'user8',	'area3','macroarea1'
	union all select 9	, 'user9',	'area3','macroarea1'
	union all select 10	, 'user10',	'area3','macroarea1'
	union all select 11	, 'user11',	'area3','macroarea2'
	union all select 12	, 'user12',	'area3','macroarea2'
	union all select 13	, 'user13',	'area3','macroarea2'
	union all select 14	, 'user14',	'area4','macroarea2'
	union all select 15	, 'user15',	'area5','macroarea2'
	union all select 16	, 'user16',	'area5','macroarea2'
	union all select 17	, 'user17',	'area5','macroarea2'
	union all select 18	, 'user18',	'area5','macroarea2'
	union all select 19	, 'user19',	'area5','macroarea2'
	union all select 20	, 'user20',	'area5','macroarea2'
END