
CREATE Procedure [dbo].[spSelDshVerficaUltimaConexion]
	@identity varchar(20),
	@ip_usuario varchar(50)=null
as
begin
	if exists(select 1 from DshAuditoria where idUserLogin=@identity and ip_usuario=@ip_usuario and pagina not in('Login','Reset','Sitio'))
	begin
		select cast((convert(float,DATEDIFF(second,max(fecha),GETDATE()))/60)as float) minutos
		from DshAuditoria where idUserLogin=@identity and ip_usuario=@ip_usuario and pagina not in('Login','Reset','Sitio')
	end
	else
		select -1 minutos
end