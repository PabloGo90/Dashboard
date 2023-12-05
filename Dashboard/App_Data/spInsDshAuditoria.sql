
create PROCEDURE [dbo].[spInsDshAuditoria]  
 @idUser [int] = NULL, --se deja que acepte nulos en el caso que accesen por la ventana'  
 @idUserLogin [nvarchar](20) = NULL, --se deja que acepte nulos en el caso que accesen por la ventana y para las casuisticas del login'  
 @ip_usuario [nvarchar](50) , -- dirección ip desde donde viene la solicitud  
 @accion [nvarchar](2) , --AG=Agrega,MO=Modifica,EL=Elimina,SE=Selecciona,LO=Login,RE=Reporte,AC=Acceso  
 @pagina [nvarchar](50) , --por éste campo rescato la actividad  
 @descripcion [nvarchar](50), -- algun comentario del acceso, por ej: No autorizado, Número de Manifiesto (si está grabando Manifiesto), Código de Sobre ( si está grabando Sobres)  
 @web [int]  --si es llamado por web = 1,si es por store procedure = 0  
AS  
BEGIN  
 declare @idPrevio int,@paginaPrevia nvarchar(50);  
 select top 1 @idPrevio=id,@paginaPrevia=pagina from DshAuditoria where convert(char(10),fecha,112)=convert(char(10),getdate(),112) and idUserLogin=@idUserLogin and ip_usuario=@ip_usuario and pagina not in('Login','Sitio','Reset','SesionExpirada') and accion
 in('AC') order by id desc;  
  
 if(isnull(@idUser,'0')!='0' and isnull(@idUserLogin,'0')='0')  
 begin try  
  if @pagina in('Login') begin  
   insert into DshAuditoria(idUserLogin,ip_usuario,fecha,accion,pagina,descripcion) values('',@ip_usuario,convert(varchar,getdate(),21),@accion,@pagina,@descripcion)  
  end else begin  
   if @idPrevio is null and @paginaPrevia is null begin  
    insert into DshAuditoria(idUserLogin,ip_usuario,fecha,accion,pagina,descripcion) values('',@ip_usuario,convert(varchar,getdate(),21),@accion,@pagina,@descripcion)  
   end else begin  
    if @idPrevio <> '' and @paginaPrevia <> '' and @pagina <> @paginaPrevia begin  
     insert into DshAuditoria(idUserLogin,ip_usuario,fecha,accion,pagina,descripcion) values('',@ip_usuario,convert(varchar,getdate(),21),@accion,@pagina,@descripcion)  
    end  
   end  
  end  
  
  if (@web = 1)  
  begin  
   select 1 as salida  
  end  
 end try  
 begin catch  
  if (@web = 1)  
  begin  
   select 0 as salida  
  end  
 end catch  
  
 else if(isnull(@idUser,'0')='0' and isnull(@idUserLogin,'0')!='0')  
 begin try  
  if @pagina in('Login') begin  
   insert into DshAuditoria(idUserLogin,ip_usuario,fecha,accion,pagina,descripcion) values(@idUserLogin,@ip_usuario,convert(varchar,getdate(),21),@accion,@pagina,@descripcion)  
  end else begin  
   if @idPrevio is null and @paginaPrevia is null begin  
    insert into DshAuditoria(idUserLogin,ip_usuario,fecha,accion,pagina,descripcion) values(@idUserLogin,@ip_usuario,convert(varchar,getdate(),21),@accion,@pagina,@descripcion)  
   end else begin  
    if @idPrevio <> '' and @paginaPrevia <> '' and @pagina <> @paginaPrevia begin  
     insert into DshAuditoria(idUserLogin,ip_usuario,fecha,accion,pagina,descripcion) values(@idUserLogin,@ip_usuario,convert(varchar,getdate(),21),@accion,@pagina,@descripcion)  
    end  
   end  
  end  
  
  if (@web = 1)  
  begin  
   select 1 as salida  
  end  
 end try  
 begin catch  
  if (@web = 1)  
  begin  
   select 0 as salida  
  end  
 end catch  
  
 else if(isnull(@idUser,'0')!='0' and isnull(@idUserLogin,'0')!='0')  
 begin try  
  if @pagina in('Login') begin  
   insert into DshAuditoria(idUser,idUserLogin,ip_usuario,fecha,accion,pagina,descripcion) values(@idUser,@idUserLogin,@ip_usuario,convert(varchar,getdate(),21),@accion,@pagina,@descripcion)  
  end else begin  
   if @idPrevio is null and @paginaPrevia is null begin  
    insert into DshAuditoria(idUser,idUserLogin,ip_usuario,fecha,accion,pagina,descripcion) values(@idUser,@idUserLogin,@ip_usuario,convert(varchar,getdate(),21),@accion,@pagina,@descripcion)  
   end else begin  
    if @idPrevio <> '' and @paginaPrevia <> '' and @pagina <> @paginaPrevia begin  
     insert into DshAuditoria(idUser,idUserLogin,ip_usuario,fecha,accion,pagina,descripcion) values(@idUser,@idUserLogin,@ip_usuario,convert(varchar,getdate(),21),@accion,@pagina,@descripcion)  
    end  
   end  
  end  
  
  if (@web = 1)  
  begin  
   select 1 as salida  
  end  
 end try  
 begin catch  
  if (@web = 1)  
  begin  
   select 0 as salida  
  end  
 end catch  
  
 if (@pagina = 'LogOut' or @descripcion = 'Sesion Expirada' or @accion = 'ER')  
 begin  
  update DshUsuarios set conectado = 0 where loginUsuario = @idUserLogin 
 end  
END  