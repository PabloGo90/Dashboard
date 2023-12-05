CREATE PROC [dbo].[spSelDshValidaCamposUsuarios]
	@loginusuario varchar(20),
	@nomusuario varchar(50) = null,
	@rutusuario varchar(10) = null,
	@email varchar(50) = null
AS
BEGIN
	if (isnull(@nomusuario,'0') != '0')
	begin
		select count(1) as salida
		from usuarios
		where nombrecompleto = @nomusuario
		  and loginusuario != @loginusuario
	end

	if (isnull(@rutusuario,'0') != '0')
	begin
		select count(1) as salida
		from usuarios
		where rut = @rutusuario
			and loginusuario != @loginusuario
	end

	if (isnull(@email,'0') != '0')
	begin
		select count(1) as salida
		from usuarios
		where correo = @email
			and loginusuario != @loginusuario
	end
END
