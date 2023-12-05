using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.CustomAuthentication;
using Dashboard.Models.Mantenedor;
using Dashboard.Helpers;


namespace Dashboard.Controllers
{
    public class DashboardCrudController : BaseController
    {
        // GET: DashboardCrud
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.DashBoardID = 0;

            if (!User.Identity.IsAuthenticated || Request.Cookies["AuthCookie"] == null)
                return RedirectToAction("LogIn", "Account");


            return View(new DashboardObj(true));
        }
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Search(string nombre)
        {
            DashboardObj dsh = new DashboardObj();
            int id = 0;
            ViewBag.DashBoardID = 0;

            if (!User.Identity.IsAuthenticated || Request.Cookies["AuthCookie"] == null)
                return RedirectToAction("LogIn", "Account");

            try
            {
                using (var db = new DashboardDL())
                {
                    dsh = db.GetDashboard(nombre, log, out id);
                    ViewBag.DashBoardID = id;
                    return View("Index", dsh);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Err DashboardController.Search");
                ModelState.AddModelError("", ex.Message);
            }
            return View("Index", new DashboardObj(true));
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Nuevo(DashboardObj dashboard)
        {
            int id = 0;
            try
            {
                dashboard.valida();

                using (var db = new DashboardDL())
                {
                    if (db.SelectIDModulo(dashboard.nombreMenu, log) != 0)
                        return Json(new { success = false, resultMsg =  $"Ya existe un dashboard con nombre de menu '{dashboard.nombreMenu}' ingresado (valor unico)" }, JsonRequestBehavior.AllowGet);

                    if (!db.InsertModulo(dashboard,log))
                        return Json(new { success = false, resultMsg =  "Error al insertar modulo (cabecera)" }, JsonRequestBehavior.AllowGet);

                    id = db.SelectIDModulo(dashboard.nombreMenu, log);
                    if (id <= 0)
                        return Json(new { success = false, resultMsg =  "Error al obtener id de nuevo modulo" }, JsonRequestBehavior.AllowGet);

                    if (!db.InsertParametros(id, dashboard.parametros, log))
                        return Json(new { success = false, resultMsg =  "Error al insertar modulo (parametros)" }, JsonRequestBehavior.AllowGet);


                    if (!db.InsertDetalle(id, dashboard.detalles, log))
                        return Json(new { success = false, resultMsg =  "Error al insertar modulo (detalles)" }, JsonRequestBehavior.AllowGet);

                    foreach (var det in dashboard.detalles)
                    {
                        int idDet = db.SelectIDModuloDetalle(det.nombreGrafico, id, log);
                        if (idDet <= 0)
                            return Json(new { success = false, resultMsg =  $"Error al obtener id de nuevo modulo detalle {det.nombreGrafico}" }, JsonRequestBehavior.AllowGet);

                        if (!db.InsertDetalleDataset(id, idDet, det.dataset, log))
                            return Json(new { success = false, resultMsg =  "Error al insertar modulo (detalleDataset)" }, JsonRequestBehavior.AllowGet);
                    }
                }

                Session[BaseController._keyCustomMenu] = null;
                return Json(new { success = true, resultMsg =  "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // rollbak
                return Json(new { success = false, resultMsg =  ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Actualiza(DashboardObj dashboard, int id)
        {
            try
            {
                dashboard.valida();

                using (var db = new DashboardDL())
                {
                    if (id <= 0)
                        return Json(new { success = false, resultMsg =  "Debe buscar/cargar modulo a actualizar" }, JsonRequestBehavior.AllowGet);

                    if(!db.UpdateModulo(id, dashboard, log))
                        return Json(new { success = false, resultMsg = "Error al actualizar modulo (cabezera)" }, JsonRequestBehavior.AllowGet);

                    db.DeleteDetails(id, log);

                    if (!db.InsertParametros(id, dashboard.parametros, log))
                        return Json(new { success = false, resultMsg =  "Error al insertar modulo (parametros)" }, JsonRequestBehavior.AllowGet);

                    if (!db.InsertDetalle(id, dashboard.detalles, log))
                        return Json(new { success = false, resultMsg =  "Error al insertar modulo (detalles)" }, JsonRequestBehavior.AllowGet);

                    foreach (var det in dashboard.detalles)
                    {
                        int idDet = db.SelectIDModuloDetalle(det.nombreGrafico, id, log);
                        if (idDet <= 0)
                            return Json(new { success = false, resultMsg =  $"Error al obtener id de nuevo modulo detalle {det.nombreGrafico}" }, JsonRequestBehavior.AllowGet);

                        if (!db.InsertDetalleDataset(id, idDet, det.dataset, log))
                            return Json(new { success = false, resultMsg =  "Error al insertar modulo (detalles)" }, JsonRequestBehavior.AllowGet);
                    }
                }

                Session[BaseController._keyCustomMenu] = null;
                return Json(new { success = true, resultMsg =  "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // rollbak
                return Json(new { success = false, resultMsg =  ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Elimina(int id)
        {
            try
            {
                if (id <= 0)
                    return Json(new { success = false, resultMsg = "Debe buscar/cargar modulo a eliminar" }, JsonRequestBehavior.AllowGet);

                using (var db = new DashboardDL())
                {
                    db.DeleteDetails(id, log);
                    db.DeleteModulo(id, log);
                }

                Session[BaseController._keyCustomMenu] = null;

                return Json(new { success = true, resultMsg =  "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // rollbak
                return Json(new { success = false, resultMsg =  ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}