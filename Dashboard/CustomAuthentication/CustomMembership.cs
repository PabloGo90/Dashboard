using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using Dashboard.CustomAuthentication;
using Dashboard.Helpers;
using Dashboard.Models;

namespace Dashboard.CustomAuthentication
{
    public class CustomMembership : MembershipProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            string Session_Ip = ConsWsDatos.obtenerip();
            String _query = "exec spSelDshValidaUsuarioLogin @identity = '" + username + "', @passw = '" + password + "',@ip_usuario = '" + Session_Ip + "', @opcion = 0";

            UsuarioSistema userLogin = ConsWsDatos.GetObjectFromDataSet<UsuarioSistema>(_query, null).FirstOrDefault();
            if (userLogin == null)
                return false;

            switch (userLogin.salida)
            {
                case -7:
                    int TUltimaAccionSis = 0;
                    double TUltimaConexion = 0;
                    int.TryParse(System.Configuration.ConfigurationManager.AppSettings["TiempoUltimaConexion"].ToString(), out TUltimaAccionSis);
                    TUltimaConexion = ConsWsDatos.TiempoUltimaAccionUsuario(username, Session_Ip);
                    if (TUltimaAccionSis == 0 || TUltimaConexion < 0 || TUltimaConexion >= TUltimaAccionSis)
                    {
                        //Si la Session esta Activa dentro de un tiempo determanido en el mismo computador,
                        //dejara ingresar nuevamente sin problemas.
                         return true;
                    }
                    else
                        throw new Exception("Usuario conectado, debe desconectarse para volver a ingresar o comunicarse con el administrador");                    
                case 0:
                    return true;
                case -1:
                    throw new Exception("Sr. usuario: su cuenta se encuentra inactiva, favor contacte al administrador del sitio");
                case -2:
                    throw new Exception("Contraseña Expirada por Intentos Fallidos/Caducidad, por favor comuniquese con el administrador.");
                case -3:
                    throw new Exception("Usuario bloqueado por cantidad de intentos fallidos de conexión, para volver a ingresar comuníquese con el administrador");
                case -4:
                    throw new Exception("Es posible que la contraseña no haya sido bien digitada. También revise si el login que ha ingresado es correcto");
                case -5:
                    throw new Exception("Es posible que la contraseña no haya sido bien digitada. También revise si el login que ha ingresado es correcto");
                case -8:
                    throw new ExcepcionCambioPassWord("La contraseña se encuentra expirada. Necesita Cambio de Contraseña");
                default:
                    break;
            }
            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if(userIsOnline)
                return new CustomMembershipUser(new UsuarioSitio(username));

            UsuarioSistema userSis = ConsWsDatos.GetObjectFromDataSet<UsuarioSistema>($"exec spSelDshUsuario 0, '{username}'",null).FirstOrDefault();

            if (userSis == null)
                throw new Exception($"No se encontro usuario {username}");
            UsuarioSitio user = new UsuarioSitio(userSis);

            return new CustomMembershipUser(user);
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        #region Overrides of Membership Provider

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}