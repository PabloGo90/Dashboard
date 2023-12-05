
CREATE proc [dbo].[spSelDshValidaUsuarioLogin]  
 @identity varchar(20),  
 @passw varchar(50),  
 @ip_usuario [nvarchar](50) = null,  
 @opcion int  
as  
begin  
 set nocount on  
 declare @passbd varchar(15),  
   @id int=0,  
   @estado integer   
  
 --inicio encriptacion  
 declare @wNombreLlave varchar(30),  
   @wPwLlave varchar(200),  
   @wSql nvarchar(4000)  
 set  @wNombreLlave = 'SymmKeyPwUsuario'  
 select @wPwLlave = valor_param from DshParametros_Sistema where variable_param = @wNombreLlave  
   
 select @wSql = 'OPEN SYMMETRIC KEY ' + @wNombreLlave + ' DECRYPTION BY PASSWORD = ''' + @wPwLlave + ''''  
 exec sp_executesql @wSql  
 --Fin encriptacion  
  
 declare @wvarintfallidos int,  
   @wdescerror varchar(100),  
   @nombreUsuario varchar(50),  
   @wusuactivo int,  
   @wcaducidad int,  
   @wdiffecha int,  
   @wdiffActivo int,  
   @winactividad int,  
   @wcantidad_passw integer,  
   @wfechacambio datetime,  
   @wusuexiste int,  
   @wsalidaaudit varchar(1),  
   @TipoAdmin bit = 0,  
   @correo varchar(50)  
  
 if ISNULL(@ip_usuario,'0') = '0'  
 begin  
  set @ip_usuario = ''  
 end  
  
 if (@opcion = 0) -- Login Usuario y Password  
 begin  
  --Valida Usuario Existente  
  select @wusuexiste = COUNT(1) from DshUsuarios where loginUsuario = @identity  
  if @wusuexiste > 0  
  begin  
   select   @passbd = CONVERT(VARCHAR(15), DECRYPTBYKEY(pass)), 
			@id=idUser,   
			@nombreUsuario=nombreCompleto, 
			@TipoAdmin=isAdmin, 
			@correo=correo,   
			@wdiffActivo=DATEDIFF(day,fechaUltimoIngreso,(getdate()-CAST((select valor_param from DshParametros_Sistema where variable_param = 'DIASINACTIVA') AS INT)))  
   from DshUsuarios 
   where loginUsuario = @identity  
   --Valida Usuario y Password  
   if @passw = @passbd  
   begin  
    -- si no es Administrador realizo validaciones correspondientes  
    if(@TipoAdmin=0)  
    begin  
     -- Valida que no esté Conectado Anteriormente  
     select @wusuactivo = COUNT(1) from DshUsuarios where loginUsuario = @identity and ISNULL(conectado,0) = 0  
     if @wusuactivo > 0  
     begin  
      --Valida Usuario Activo  
      select @wusuactivo = COUNT(1) from DshUsuarios where loginUsuario = @identity and ISNULL(activo,0) = 1  
      if @wusuactivo > 0  
      begin  
       if(@wdiffActivo>=0)  
       begin  
        update DshUsuarios  
        set activo=0  
        where loginUsuario = @identity  
        --usuario inactivo  
        set @estado = -1  
        exec spInsDshAuditoria @idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Inactivo',@web=0  
       end  
       else  
       begin  
        --Valida Caducidad de Password  
        select @wcaducidad = ISNULL(caducidad,0) from DshUsuarios where loginUsuario = @identity  
        if @wcaducidad = 0  
        begin  
         -- Valida getdate - FechaExpiracion no menor a cero  
         select @wdiffecha = DATEDIFF(day,FechaExpiracion,getdate()) from DshUsuarios where Iduser = @id  
         if @wdiffecha >= 0  
         begin  
          --usuario queda expirado/caducado debe cambiar la password  
          set @estado = -8  
          exec spInsDshAuditoria @idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Contraseña expirada',@web=0  
         end  
         else  
         begin  
          -- usuario aun no expira  
          set @estado = 0  
          update DshUsuarios set intentos_fallidos=0, fechaUltimoIngreso = GETDATE(), conectado = 1 where loginUsuario = @identity;  
          exec spInsDshAuditoria @idUser=@id,@idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Logueado',@web=0;  
         end   
        end  
        else  
        begin  
         --usuario se encuentra caducado  
         set @estado = -2  
         exec spInsDshAuditoria @idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Caducado',@web=0  
        end  
       end  
      end  
      else  
      begin  
       --usuario inactivo  
       set @estado = -1  
       exec spInsDshAuditoria @idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Inactivo',@web=0  
      end  
     end  
     else  
     begin  
      --usuario ya conectado  
      set @estado = -7  
      exec spInsDshAuditoria @idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Ya Conectado Anteriormente',@web=0  
     end  
    end  
    else  
    begin  
     set @estado = 0  
     exec spInsDshAuditoria @idUser=@id,@idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Logueado',@web=0;  
    end  
   end  
   else  
   begin  
    -- si no es Administrador realizo validaciones correspondientes  
    if(@TipoAdmin=0)  
    begin  
     --Valida Intentos Fallidos  
     set @wvarintfallidos = (SELECT ISNULL(intentos_fallidos, 0) + 1 FROM DshUsuarios WHERE loginUsuario = @identity)  
     if  @wvarintfallidos > (select valor_param from DshParametros_Sistema where variable_param = 'MAXINTENTOS')  
     begin  
      --usuario se bloquea por tener máximo de intentos  
      set @estado = -3  
      update DshUsuarios set activo = 0,intentos_fallidos = ISNULL(intentos_fallidos, 0) + 1 where loginUsuario = @identity  
      exec spInsDshAuditoria @idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Bloqueado MAXINTENTOS',@web=0  
     end  
     else  
     begin  
      --usuario/contraseña inválidos  
      set @estado = -4  
      update DshUsuarios set intentos_fallidos = ISNULL(intentos_fallidos, 0) + 1 WHERE loginUsuario = @identity  
      exec spInsDshAuditoria @idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Usuario/Contraseña Inválidos',@web=0  
     end  
    end  
    else  
    begin  
     set @estado = -4  
     exec spInsDshAuditoria @idUser=@id,@idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Logueado',@web=0;  
    end  
   end  
  end  
  else  
  begin  
   --usuario no existe  
   set @estado = -5  
   exec spInsDshAuditoria @idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Usuario No Existe',@web=0  
  end  
  
 end  
 if (@opcion = 1) -- Cambio de Password  
 begin  
  --Valida si la nueva Password se encuentra dentro de la tabla historica de password  
  select @wcantidad_passw = count(1) from hist_passw where id_usuario = (select IdUser from DshUsuarios where loginUsuario = @identity) and @passw = CONVERT(VARCHAR(15), DECRYPTBYKEY(passwd_usuario ))  
  if @wcantidad_passw > 0  
  begin  
   --Password se encuentra en Tabla Historica , usuario debe colocar otra  
   exec spInsDshAuditoria @idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Contraseña en Historica',@web=0  
   set @estado = -1  
  end  
  else  
  begin  
   --Valida que la cantidad de Password guardadas no superen lo que indica el Parámetro  
   select @wcantidad_passw = count(1) from hist_passw where id_usuario = (select IdUser from DshUsuarios where loginUsuario = @identity)  
   if @wcantidad_passw = (select valor_param from DshParametros_Sistema where variable_param = 'CANTIDADHISTORICOS')  
   begin  
    --Elimina el Registro más antiguo  
    select top(1) @wfechacambio = fecha_cambio_clave from hist_passw where id_usuario = (select IdUser from DshUsuarios where loginUsuario = @identity) ORDER BY fecha_cambio_clave ASC  
    delete from hist_passw where id_usuario = (select IdUser from DshUsuarios where loginUsuario = @identity) and fecha_cambio_clave = @wfechacambio      
   end  
   update DshUsuarios set activo=1,caducidad = 0,intentos_fallidos=0,pass= EncryptByKey(Key_GUID(@wNombreLlave),@passw),FechaActivacion = GETDATE(),FechaExpiracion = GETDATE() + 30 WHERE loginUsuario = @identity  
   insert into hist_passw values((select IdUser from DshUsuarios where loginUsuario = @identity),getdate(),EncryptByKey(Key_GUID(@wNombreLlave),@passw))  
   exec spInsDshAuditoria @idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='MO',@pagina='MantenedorUsuarios',@descripcion='Contraseña Cambiada',@web=0  
   set @estado = 0  
  end  
 end  
 if (@opcion = 2) -- Cambio de Password desde un Store Procedure  
 begin  
  --Valida si la nueva Password se encuentra dentro de la tabla historica de password  
  select @wcantidad_passw = count(1) from hist_passw where id_usuario = (select IdUser from DshUsuarios where loginUsuario = @identity) and @passw = CONVERT(VARCHAR(15), DECRYPTBYKEY(passwd_usuario ))  
  if @wcantidad_passw > 0  
  begin  
   --Password se encuentra en Tabla Historica , usuario debe colocar otra  
   exec spInsDshAuditoria @idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='AC',@pagina='Login',@descripcion='Contraseña en Historica',@web=0  
   set @estado = -1  
  end  
  else  
  begin  
   --Valida que la cantidad de Password guardadas no superen lo que indica el Parámetro  
   select @wcantidad_passw = count(1) from hist_passw where id_usuario = (select IdUser from DshUsuarios where loginUsuario = @identity)  
   if @wcantidad_passw = (select valor_param from DshParametros_Sistema where variable_param = 'CANTIDADHISTORICOS')  
   begin  
    --Elimina el Registro más antiguo  
    select top(1) @wfechacambio = fecha_cambio_clave from hist_passw where id_usuario = (select IdUser from DshUsuarios where loginUsuario = @identity) ORDER BY fecha_cambio_clave ASC  
    delete from hist_passw where id_usuario = (select IdUser from DshUsuarios where loginUsuario = @identity) and fecha_cambio_clave = @wfechacambio      
   end  
   update DshUsuarios set activo=1,caducidad = 0,intentos_fallidos=0,pass= EncryptByKey(Key_GUID(@wNombreLlave),@passw),FechaActivacion = GETDATE(),FechaExpiracion = GETDATE() + 30 WHERE loginUsuario = @identity  
   insert into hist_passw values((select IdUser from DshUsuarios where loginUsuario = @identity),getdate(),EncryptByKey(Key_GUID(@wNombreLlave),@passw))  
   exec spInsDshAuditoria @idUserLogin=@identity,@ip_usuario=@ip_usuario,@accion='MO',@pagina='MantenedorDshUsuarios',@descripcion='Contraseña Cambiada',@web=0  
   set @estado = 0;  
  end  
  
 end  
   
 select @estado as salida, @nombreUsuario as nombreusuario, @id idUsr, @TipoAdmin TipoAdmin, @correo correo  
 set nocount off  
end  