CREATE PROCEDURE [dbo].[spSelDshUsuario]
	@id int,
	@user varchar(50) = null
	 AS
BEGIN
	select idUser
		,nombreCompleto
		,pass
		,rut
		,activo
		,loginUsuario
		,fechaUltimoIngreso
		,correo
		,fono
		,ip_usuario
		,caducidad
		,intentos_fallidos
		,FechaActivacion
		,FechaExpiracion
		,conectado
		,isAdmin
	FROM DshUsuarios
	WHERE (@id <> 0 and idUser = @id)
	OR    (@id = 0 and (loginUsuario like '%' + @user + '%' OR
					   nombreCompleto like '%' + @user + '%' ))
END