using Dashboard.CustomAuthentication;
using Dashboard.Helpers;
using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    public class HomeController : BaseController
    {
        [CustomAuthorize(Roles = "Home")]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated || Request.Cookies["AuthCookie"] == null)
                return RedirectToAction("LogIn", "Account");

            List<ModHome> home = new List<ModHome>();
            try
            {
                home = ConsWsDatos.GetObjectFromDataSet<ModHome>("exec spSelDshHome",log).OrderBy(o => o.Orden).ToList();
            }
            catch (Exception ex)
            {
                log.Error($"HomeController.Index {ex.ToString()}");
                ModelState.AddModelError("", ex.Message);
            }
            finally
            {
                ConsWsDatos.ExecuteNonQuery($"exec spInsDshAuditoria @idUser = null,@idUserLogin = '{_User.UserName}',@ip_usuario ='{ConsWsDatos.obtenerip()}',@accion = 'AC',@pagina = 'Home',@descripcion = 'Acceso a home',@web = 1", log);                
            }

            return View(home);

        }
    }
}