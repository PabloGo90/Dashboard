using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Dashboard.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error()
        {
            ViewBag.ErrorMsg = "Error";
            return View();
        }
        public ActionResult AccessExpired()
        {
            HttpCookie cookie = new HttpCookie("AuthCookie", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            ViewBag.ErrorMsg = "Sesión expirada";
            return View("ErrorGoLogin");
        }
        // GET: Error
        public ActionResult AccessDenied()
        {
            ViewBag.ErrorMsg = "Error acceso";
            return View("ErrorGoLogin");
        }
        // GET: Error
        public ActionResult NotFound()
        {
            ViewBag.ErrorMsg = "No encontrado";
            return View("Error");
        }
        // GET: Error
        public ActionResult Oops(string msg)
        {
            ViewBag.ErrorMsg = msg;
            return View("Error");
        }
    }
}