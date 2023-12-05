using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dashboard.Models;
using System.Web.Mvc;
using Dashboard.Helpers;
using Dashboard.CustomAuthentication;

namespace Dashboard.Controllers
{
    public class UserController : BaseController
    {

        // GET: User
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(new List<UsuarioSistema>().AsEnumerable());
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Search(string username)
        {
            try
            {
                return View("Index", ConsWsDatos.GetObjectFromDataSet<UsuarioSistema>($"exec spSelDshUsuario 0, '{username}'", log));
            }
            catch (Exception ex)
            {
                log.Error(ex, "Err UserController.Search");
                ModelState.AddModelError("", ex.Message);
            }

            return View("Index", new List<UsuarioSistema>());
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult PostCreate(UsuarioSistema usuario)
        {
            try
            {
                
                if (!ValidarInputs(usuario, true))
                    return View("Create", usuario);
                

                ConsWsDatos.ExecuteNonQuery(string.Format("exec spInsDshUsuario '{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},'{8}','{9}'",
                                                usuario.nombreCompleto,
                                                usuario.pass,
                                                usuario.rut,
                                                usuario.loginUsuario,
                                                usuario.correo,
                                                usuario.fono,
                                                usuario.activo ? 1 : 0,
                                                usuario.isAdmin ? 1 : 0,
                                                ConsWsDatos.obtenerip(),
                                                _User.UserName), log);

            }
            catch (Exception ex)
            {
                log.Error(ex, "Err UserController.PostCreate");
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.ShowSuccessMsg = "Y";
            return View("Create", new UsuarioSistema());
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                return View(ConsWsDatos.GetObjectFromDataSet<UsuarioSistema>($"exec spSelDshUsuario {id}", log).FirstOrDefault());
            }
            catch (Exception ex)
            {
                log.Error(ex, "Err UserController.Edit");
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult PostEdit(UsuarioSistema usuario)
        {
            try
            {

                if (!ValidarInputs(usuario))
                    return View("Edit", usuario);
                
                ConsWsDatos.ExecuteNonQuery(string.Format("exec spUpdDshUsuario '{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},'{8}','{9}',{10}",
                                                usuario.nombreCompleto,
                                                usuario.pass,
                                                usuario.rut,
                                                usuario.loginUsuario,
                                                usuario.correo,
                                                usuario.fono,
                                                usuario.activo ? 1 : 0,
                                                usuario.isAdmin ? 1 : 0,
                                                ConsWsDatos.obtenerip(),
                                                _User.UserName,
                                                usuario.flagCambioPwd ? 1 : 0), log);

            }
            catch (Exception ex)
            {
                log.Error(ex, "Err UserController.PostEdit");
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.ShowSuccessMsg = "Y";
            return View("Edit",usuario);
        }


        private bool ValidarInputs(UsuarioSistema usuario, bool isNewUsr = false)
        {
            string MsjOut = string.Empty;
            if (!ConsWsDatos.ValidaCampo(usuario.nombreCompleto, usuario.loginUsuario, "nombre", _strRgxNom, out MsjOut))
            {
                ModelState.AddModelError("", MsjOut);
                return false;
            }
            if (!ConsWsDatos.ValidaCampo(usuario.rut, usuario.loginUsuario, "rut", _strRgxRut, out MsjOut))
            {
                ModelState.AddModelError("", MsjOut);
                return false;
            }
            if(isNewUsr && !usuario.flagCambioPwd)
            {
                ModelState.AddModelError("", "Debe proporcionar una contraseña");
                return false;
            }
            if (usuario.flagCambioPwd && usuario.pass != usuario.pass2)
            {
                ModelState.AddModelError("", "Contraseña no coincide");
                return false;
            }
            if (usuario.flagCambioPwd && !ConsWsDatos.ValidaCampo(usuario.pass, usuario.loginUsuario, "contraseña", _strRgxPwd, out MsjOut))
            {
                ModelState.AddModelError("", MsjOut);
                return false;
            }
            if (!ConsWsDatos.ValidaCampo(usuario.correo, usuario.loginUsuario, "mail", _strRgxMail, out MsjOut))
            {
                ModelState.AddModelError("", MsjOut);
                return false;
            }
            return true;
        }
    }
}