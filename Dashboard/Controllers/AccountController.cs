using Dashboard.CustomAuthentication;
using Dashboard.Helpers;
using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Dashboard.Controllers
{
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult LogIn(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = returnUrl;

            if (System.Configuration.ConfigurationManager.AppSettings["LoginExterno"].ToUpper() == "Y")
                return View("ExternalLogin");
            else
                return View("LogIn");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginViewModel loginView, string ReturnUrl = "")
        {
            bool validadoExterno = false;
            if (ModelState.IsValid)
            {
                try
                {
                    bool loginExterno = System.Configuration.ConfigurationManager.AppSettings["LoginExterno"].ToUpper() == "Y";
                    if (loginExterno)
                    {
                        validadoExterno = (Session["usuariosic"] ?? "").ToString() == loginView.Usuario && !string.IsNullOrEmpty(loginView.Usuario);
                        //Se comenta codigo que solo permite login externo cuando asi esta configurado. se deja ambos metodos de acceso
                        //if (!validado)
                        //{
                        //    ModelState.AddModelError("", "Inicio de sesión es valido solo mediante portal SIC");
                        //    log.Error("Intento de login. Inicio de sesión es valido solo mediante portal SIC");
                        //    return View(loginView);
                        //}

                        if (!validadoExterno && string.IsNullOrEmpty(loginView.Usuario))
                        {
                            ModelState.AddModelError("", "Error al inicar sesión por portal Sic");
                            return View(new LoginViewModel());
                        }
                    }

                    //CUSTOM MEMBERSHIP >
                    if (validadoExterno || Membership.ValidateUser(loginView.Usuario, loginView.Password))
                    {
                        var user = (CustomMembershipUser)Membership.GetUser(loginView.Usuario, validadoExterno);
                        this.CreateAccessToken(user);

                        if (Url.IsLocalUrl(ReturnUrl))
                            return Redirect(ReturnUrl);
                        else
                            return RedirectToAction("Index", string.IsNullOrEmpty(user.DefaultView) ? "Home" : user.DefaultView);
                    }
                    else
                        ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                }
                catch (ExcepcionCambioPassWord expwd)
                {
                    ModelState.AddModelError("", expwd.Message);
                    //Genera token para acceso solo a pagina cambio de password
                    var user = (CustomMembershipUser)Membership.GetUser(loginView.Usuario, false);
                    this.CreateAccessToken(user, true);
                    return RedirectToAction("ChangePwd", "Account");
                }
                catch (Exception ex)
                {
                    log.Error(ex, "Error al iniciar sesion");
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
                ModelState.AddModelError("", "Login no valido");

            return View(loginView);
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginBlank()
        {
            List<string> ListaTipoSIC = new List<string> { "PUBLICO", "PRIVADO" };
            List<string> ListaQuerySIC = new List<string> { "usr", "usuario" };
            string stridusuariosic = "";
            string sTipoSIC = System.Configuration.ConfigurationManager.AppSettings["TipoSIC"].ToUpper();

            #region ExternalLogin
            if (Request.UrlReferrer != null)
                varurl = Request.UrlReferrer.AbsoluteUri.ToString();
            else
                varurl = Request.Url.ToString();

            varuser = sTipoSIC == "PUBLICO" ? Request.QueryString["usr"] ?? "" : "";
            varusuario = Request.QueryString["usuario"] ?? "";
            varsucursal = Request.QueryString["sucursal"] ?? "";
            varapl = Request.QueryString["apl"] ?? "";

            string ip = ConsWsDatos.obtenerip();
            if (varuser.Length > 0) Session["usr"] = varuser;
            if (varusuario.Length > 0) Session["usuario"] = varusuario;
            if (varsucursal.Length > 0) Session["sucursal"] = varsucursal;
            if (varapl.Length > 0) Session["apl"] = varapl;
            if (ip.Length > 0) Session["ip"] = ip;

            //ValidarSIC
            if (ListaTipoSIC.Contains(sTipoSIC))
            {
                string sQueryUsuarioSIC = ListaQuerySIC[ListaTipoSIC.IndexOf(sTipoSIC)];
                //stridusuariosic = page.Request.QueryString[sQueryUsuarioSIC];
                if (Session[sQueryUsuarioSIC.ToString()] != null)
                    stridusuariosic = Session[sQueryUsuarioSIC.ToString()].ToString();

                if (stridusuariosic.IndexOf(",") > -1)
                    stridusuariosic = stridusuariosic.Substring(0, stridusuariosic.IndexOf(","));
            }
            if (!string.IsNullOrEmpty(stridusuariosic)) Session["usuariosic"] = stridusuariosic;

            //ViewBag.varusuario = varusuario;
            log.Debug(string.Format("Sessions: usr {0}; usuario {1}; sucursal {2}; apl {3}; ip {4}; usuariosic {5}", varuser, varusuario, varsucursal, varapl, ip, stridusuariosic));
            #endregion

            return View();
        }

        [AllowAnonymous]
        public ActionResult ExternalLogin()
        {
            ViewBag.UsuarioSic = Session["usuariosic"];

            if (System.Configuration.ConfigurationManager.AppSettings["LoginExterno"].ToUpper() == "Y")
                return View("ExternalLogin");
            else
                return View("LogIn");
        }

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("AuthCookie", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();

            ConsWsDatos.ExecuteNonQuery($"exec spInsDshAuditoria @idUser = null,@idUserLogin = '{_User.UserName}',@ip_usuario ='{ConsWsDatos.obtenerip()}',@accion = 'AC',@pagina = 'LogOut',@descripcion = 'Deslogueado',@web = 1", log);


            if (System.Configuration.ConfigurationManager.AppSettings["LoginExterno"].ToUpper() == "Y")
                return RedirectToAction("Close", "Account");
            else
                return RedirectToAction("LogIn", "Account", null);
        }

        [AllowAnonymous]
        public ActionResult Close()
        {
            return View();
        }

        [CustomAuthorize(Roles = "ChangePwd")]
        public ActionResult ChangePwd()
        {
            if (string.IsNullOrEmpty(_User.UserName))
                ModelState.AddModelError("", "Error al obtener usuario");

            @ViewBag.UserName = _User.UserName;
            return View();
        }
        [HttpPost]

        [CustomAuthorize(Roles = "ChangePwd")]
        public ActionResult ChangePwd(CambioContrasena cambio)
        {
            string MsjOut;
            @ViewBag.UserName = _User.UserName;

            if (string.IsNullOrEmpty(cambio.username) || _User.UserName != cambio.username)
            {
                ModelState.AddModelError("", "No se carga usuario o usuario distinto");
                return View("ChangePwd");
            }
            if (cambio.pwdNew != cambio.pwdNew2)
            {
                ModelState.AddModelError("", "Contraseña no coincide");
                return View("ChangePwd");
            }
            if (!ConsWsDatos.ValidaCampo(cambio.pwdNew, cambio.username, "contraseña", _strRgxPwd, out MsjOut))
            {
                ModelState.AddModelError("", MsjOut);
                return View("ChangePwd");
            }

            string Session_Ip = ConsWsDatos.obtenerip();
            String _query = "exec spSelDshValidaUsuarioLogin @identity = '" + cambio.username +
                            "', @passw = '" + cambio.pwdNew + "',@ip_usuario = '" + Session_Ip + "', @opcion = 1";

            UsuarioSistema userLogin = ConsWsDatos.GetObjectFromDataSet<UsuarioSistema>(_query, null).FirstOrDefault();
            if (userLogin == null)
            {
                ModelState.AddModelError("", "Error al validar nueva contraseña");
                return View("ChangePwd");
            }

            if (userLogin.salida == -1)
            {
                ModelState.AddModelError("", "Contraseña ya fue utilizada anteriormente, porfavor ocupe otra.");
                return View("ChangePwd");
            }

            //Cambio de clave correcto, vuelve a generar token y redirecciona a Home
            var user = (CustomMembershipUser)Membership.GetUser(cambio.username, false);
            this.CreateAccessToken(user);
            return RedirectToAction("Index", string.IsNullOrEmpty(user.DefaultView) ? "Home" : user.DefaultView);
        }

        private void CreateAccessToken(CustomMembershipUser user, bool changePwd = false)
        {
            int exp = 0;
            if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["TicketTiempoExpiracion"], out exp))
                exp = 10;

            CustomSerializeModel userModel = new Models.CustomSerializeModel()
            {
                UserId = user.UserId,
                UserName = user.Name, //UserName = loginView.Usuario,
                Roles = user.Funciones.Where(x => !changePwd || (changePwd && x.RoleName == "ChangePwd"))
                                        .Select(r => r.RoleName).ToList(),
                DefaultView = user.DefaultView
            };

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                (
                    1,
                    user.UserName,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(exp),
                    false,
                    Newtonsoft.Json.JsonConvert.SerializeObject(userModel)
                );


            //Genera cookie authorization
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
            faCookie.Expires = authTicket.Expiration;
            Response.Cookies.Add(faCookie);

            //sesion usuario
            CurrentUser = user.Name;
        }

    }
}