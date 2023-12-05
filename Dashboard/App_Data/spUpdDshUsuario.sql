
CREATE procedure [dbo].[spUpdDshUsuario]  
 @nombreCompleto nvarchar(300),  
 @pass varchar(15),  
 @rut nvarchar(20),  
 @loginUsuario varchar(20),  
 @correo nvarchar(50),  
 @fono nvarchar(50),  
 @activo bit,   
 @isAdmin bit,
 @ip_usuario nvarchar(50),  
 @loginUsuariodesde varchar(20) = null, 
 @flagcambiopassword tinyint
as  
begin  
 declare @existe integer  
 declare @wdes varchar(200)  
 declare @widUser integer  
  
 --inicio encriptacion  
 declare @wNombreLlave varchar(30)  
 declare @wPwLlave varchar(200)  
 declare @wSql nvarchar(4000)  
 set  @wNombreLlave = 'SymmKeyPwUsuario'  
 select @wPwLlave = valor_param from DshParametros_Sistema where variable_param = @wNombreLlave  
   
 select @wSql = 'OPEN SYMMETRIC KEY ' + @wNombreLlave + ' DECRYPTION BY PASSWORD = ''' + @wPwLlave + ''''  
 exec sp_executesql @wSql  
 --Fin encriptacion   
 
 select @existe = COUNT(1) from DshUsuarios where loginUsuario = @loginUsuario  
  
	if (@existe = 0)  
		RAISERROR('Usuario no existe',11,1);

    UPDATE DshUsuarios  
    SET nombreCompleto = @nombreCompleto,   
     pass   = case when @flagcambiopassword = 1 then EncryptByKey(Key_GUID(@wNombreLlave), @pass) else pass end,  
     rut    = @rut,   
     activo   = @activo,  
     correo   = @correo,   
     fono   = @fono,   
     intentos_fallidos = 0,  
     FechaActivacion = GETDATE(),  
     isAdmin = @isAdmin,  
	 fechaUltimoIngreso = case when loginUsuario = @loginUsuariodesde then getdate() else fechaUltimoIngreso end,
	 FechaExpiracion = case 
						when @flagcambiopassword = 0 then FechaExpiracion
						when loginUsuario = @loginUsuariodesde then (GETDATE() + CAST((select valor_param from DshParametros_Sistema where variable_param = 'DIASEXPIRACIONPWD') AS INT))   
						else (GETDATE() - 1) end
    WHERE loginUsuario = @loginUsuario  


	if (@flagcambiopassword = 1 )
	  insert into DshHist_passw(id_usuario, fecha_cambio_clave, passwd_usuario)
	  values( (select idUser from DshUsuarios where loginUsuario = @loginUsuario), GETDATE(), EncryptByKey(Key_GUID(@wNombreLlave), @pass))  

      
    set @wdes = (select 'Usuario Actualizado por ' + @loginUsuariodesde)  
    set @widUser = (select idUser from DshUsuarios where loginUsuario = @loginUsuariodesde)  
    exec spInsDshAuditoria @idUser=@widUser,@idUserLogin=@loginUsuario,@ip_usuario=@ip_usuario,@accion='MO',@pagina='MantenedorUsuarios',@descripcion=@wdes,@web=0  

end  