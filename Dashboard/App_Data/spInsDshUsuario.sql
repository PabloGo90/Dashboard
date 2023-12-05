CREATE procedure [dbo].[spInsDshUsuario]  
 @nombreCompleto nvarchar(300),  
 @pass varchar(15),   
 @rut nvarchar(20),  
 @loginUsuario varchar(20),  
 @correo nvarchar(50),  
 @fono nvarchar(50),  
 @activo bit,  
 @isAdmin bit,
 @ip_usuario [nvarchar](50) = null,  
 @loginUsuariodesde varchar(20) = null 
as  
begin  
 set nocount on  
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
  
	if (@existe = 1)  
		RAISERROR('Usuario ya existe',11,1);

  insert into DshUsuarios(  
	   nombreCompleto,   
	   pass,   
	   rut,   
	   activo,   
	   loginUsuario,   
	   correo,   
	   fono,   
	   FechaActivacion,  
	   FechaExpiracion,  
	   isAdmin,  
	   caducidad 
  )values(  
   @nombreCompleto,   
   EncryptByKey(Key_GUID(@wNombreLlave), @pass),  
   @rut,   
   @activo,   
   @loginUsuario,   
   @correo,   
   @fono,   
   GETDATE(),  
   (GETDATE() + CAST((select valor_param from DshParametros_Sistema where variable_param = 'DIASEXPIRACIONPWD') AS INT)),  
   @isAdmin,  
   0
  )  
  
  insert into DshHist_passw(id_usuario, fecha_cambio_clave, passwd_usuario)
  values( (select idUser from DshUsuarios where loginUsuario = @loginUsuario), GETDATE(), EncryptByKey(Key_GUID(@wNombreLlave), @pass))  
  
  set @wdes = (select 'Usuario Creado por ' + @loginUsuariodesde)  
  set @widUser = (select idUser from DshUsuarios where loginUsuario = @loginUsuariodesde)  
  exec spInsDshAuditoria @idUser=@widUser,@idUserLogin=@loginUsuario,@ip_usuario=@ip_usuario,@accion='AG',@pagina='MantenedorUsuarios',@descripcion=@wdes,@web=0  

end  