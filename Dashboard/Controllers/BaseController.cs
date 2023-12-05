using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Serilog;
using Dashboard.Helpers;

namespace Dashboard.Controllers
{
    public class BaseController : Controller
    {
        public CustomSerializeModel _User = null;
        public static readonly Serilog.ILogger log = SerilogClass._log;
        private const string _keyBaseUrl = "BaseUrl";
        public const string _keyCustomMenu = "CustomMenu";
        public const string _keyCustColor = "CustColor";
        public const string _keyCustColorLight = "CustColorLight";
        public const string _keyImgEmpresa = "ImgEmpresa";
        public const string _keyCurrentUser = "CurrentUser";
        public const string _strRgxLog = "[a-zA-Z0-9_]{1,15}";
        public const string _strRgxNom = "[ a-zA-ZáéíóúÁÉÍÓÚñÑ]{1,50}";
        public const string _strRgxRut = "^\\d{7,8}[-][0-9kK]{1}$";
        public const string _strRgxPwd = "^(?=.*\\d)(?=.*[\\u0021-\\u002b\\u003c-\\u0040])(?=.*[A-Z])(?=.*[a-z])\\S{8,15}$";
        public const string _strRgxMail = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        public string varusuario = "";
        public string varsucursal = "";
        public string varapl = "";
        public string varurl = "";
        public string varip = "";
        public string varuser = "";

        public string BaseUrl { get { return Session[_keyBaseUrl] as string; } set { Session[_keyBaseUrl] = value; } }
        public string CustColor { get { return Session[_keyCustColor] as string; } set { Session[_keyCustColor] = value; } }
        public string CustColorLight { get { return Session[_keyCustColorLight] as string; } set { Session[_keyCustColorLight] = value; } }
        public string ImgEmpresa { get { return Session[_keyImgEmpresa] as string; } set { Session[_keyImgEmpresa] = value; } }
        public string CurrentUser { get { return Session[_keyCurrentUser] as string; } set { Session[_keyCurrentUser] = value; } }
        public List<ModMenu> CustomMenu  { get { return Session[_keyCustomMenu] as List<ModMenu>; } set { Session[_keyCustomMenu] = value; }}


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int idDashboard;
            string msgCache = "";
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //GC.Collect();
            try
            {
                ViewBag.CompanyName = "TCS";

                #region Sessions config
                if (string.IsNullOrEmpty(BaseUrl))
                {
                    BaseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
                    msgCache += "- Base url: " + BaseUrl;
                }
                if (string.IsNullOrEmpty(CustColor))
                {
                    CustColor = System.Configuration.ConfigurationManager.AppSettings["SiteColor"];
                    msgCache += "- CustColor: " + CustColor;
                }
                if (string.IsNullOrEmpty(CustColorLight))
                {
                    CustColorLight = System.Configuration.ConfigurationManager.AppSettings["SiteColorLight"];
                    msgCache += "- CustColorLight: " + CustColorLight;
                }
                if (string.IsNullOrEmpty(ImgEmpresa))
                {
                    ImgEmpresa = System.Configuration.ConfigurationManager.AppSettings["ImgEmpresa"];
                    msgCache += "- ImgEmpresa: " + ImgEmpresa;
                }
                if (!string.IsNullOrEmpty(msgCache))
                    log.Debug($"Estableciendo configuracion variables cache ({msgCache})");

                ViewBag.BaseUrl = BaseUrl;
                ViewBag.CustColor = CustColor;
                ViewBag.CustColorLight = CustColorLight;
                ViewBag.ImgEmpresa = ImgEmpresa;
                #endregion

                ViewBag.CompanyName = "TCS";

                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                    _User = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomSerializeModel>(authTicket.UserData);

                    ViewBag.WebUser = _User.UserName;
                    ViewBag.UserName = _User.Ubicacion;
                    ViewBag.ShowCrud = _User.Roles.Exists(x => x == "Admin") ? "Y" : "N";
                    ViewBag.IsPortalSicUser = _User.Roles.Exists(x => x == "PortalSic") ? "Y" : "N";

                    #region menu
                    if (CustomMenu == null || CustomMenu.Count == 0)
                        CustomMenu = GetMenuList();
                    int.TryParse(Request.QueryString["ID"], out idDashboard);

                    //Define menu activo 
                    CustomMenu.ForEach(x => x.linkActive = (!x.isParent && x.Id == idDashboard));
                    CustomMenu.ForEach(x => x.ChildMenus.ForEach(y => y.linkActive = (y.Id == idDashboard)));
                    CustomMenu.ForEach(x => x.collapse = (x.isParent && x.ChildMenus.Exists(y => y.linkActive)));

                    ViewBag.CustomMenu = CustomMenu;
                    ViewBag.HomeActive = idDashboard == 0;
                    #endregion                    
                }
                else if (!string.IsNullOrEmpty(CurrentUser))
                {
                    log.Error("Cookie AuthCookie no existe. cerrando sesion.");
                    ConsWsDatos.ExecuteNonQuery($"exec spInsDshAuditoria @idUser = null,@idUserLogin = '{CurrentUser}',@ip_usuario ='{ConsWsDatos.obtenerip()}',@accion = 'AC',@pagina = 'LogOut',@descripcion = 'Deslogueado',@web = 1", log);
                    CurrentUser = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Err OnActionExecuting");
                throw new HttpException(500, ex.Message);
            }
        }
        private List<ModMenu> GetMenuList()
        {
            List<ModMenu> menuList = new List<ModMenu>();
            List<ModMenuChild> aux = new List<ModMenuChild>();
            int i = 1;
            try
            {
                aux = ConsWsDatos.GetObjectFromDataSet<ModMenuChild>("exec spSelDshMenu", log).OrderBy(o => o.Orden).ToList();

                foreach (ModMenuChild menu in aux)
                {
                    if (!string.IsNullOrEmpty(menu.Parent) && !menuList.Exists(x => x.Nombre == menu.Parent))
                        menuList.Add(new ModMenu(aux, menu.Parent, i));
                    else if (string.IsNullOrEmpty(menu.Parent))
                        menuList.Add(new ModMenu(menu, i));

                    i++;
                }

                return menuList;
            }
            catch (Exception ex)
            {
                log.Error($"BaseController.GetMenuList {ex.ToString()}");
                return new List<ModMenu>();
            }
        }
    }
}
