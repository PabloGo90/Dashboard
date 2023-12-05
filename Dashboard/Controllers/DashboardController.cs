using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.Models;
using System.Data;
using Newtonsoft.Json;
using System.Globalization;
using Dashboard.CustomAuthentication;
using Dashboard.Helpers;

namespace Dashboard.Controllers
{
    public class DashboardController : BaseController
    {
        [CustomAuthorize(Roles = "Dashboard")]
        public ActionResult Index(int id = 0)
        {
            if (!User.Identity.IsAuthenticated || Request.Cookies["AuthCookie"] == null)
                return RedirectToAction("LogIn", "Account");

            try
            {
                return View(id == 0 ? new ModInfo() : GetModInfo(id));
            }
            catch (Exception ex)
            {
                log.Error(ex, "Err DashboardController.Index");
                ModelState.AddModelError("", ex.Message);
            }
            return View(new ModInfo());
        }
        [CustomAuthorize(Roles = "Dashboard")]
        public ActionResult Search(int id)
        {
            if (!User.Identity.IsAuthenticated || Request.Cookies["AuthCookie"] == null)
                return RedirectToAction("LogIn", "Account");

            try
            {
                return View("Index", GetModInfo(id, true));
            }
            catch (Exception ex)
            {
                log.Error(ex, "Err DashboardController.Search");
                ModelState.AddModelError("", ex.Message);
            }
            return View("Index", new ModInfo());
        }

        private ModInfo GetModInfo(int id, bool isUserSearch = false)
        {
            ModInfo mod = new ModInfo();

            #region Render parametros
            //Busca por id y construye modulo
            mod.mapModInfo(ConsWsDatos.GetDataSet($"exec spSelDshModuloInfo {id}", log));

            if (!mod.activo)
                throw new Exception("Dashboard es encuentra inactivo!");

            //llena combobox
            if (mod.Parametros.Count > 0)
                mod.Parametros.Where(x => x.TipoHtml == AppEnums.ParamTipoHtml.SELECT).ToList()
                          .ForEach(par => par.setModParametrosCombos(ConsWsDatos.GetDataSet($"exec {par.selectListSPData}", log)));
            #endregion

            if (isUserSearch && mod.Parametros.Count > 0)
            {
                //Actualiza VALOR de parametros segun valor ingresado por el usuario (rescata desde la url)
                mod.Parametros.ForEach(x => x.Valor = Request.QueryString[x.NombreParSP]);
                //Marca opcion seleccionada por usuario (campos select) 
                mod.Parametros.Where(x => x.TipoHtml == AppEnums.ParamTipoHtml.SELECT).ToList()
                              .ForEach(par => par.setSelectedParametrosCombos());

                //Valida parametros
                using (ValidaParametros validaParametros = new ValidaParametros(log, mod.Parametros))
                {
                    if (!validaParametros.Validar())
                    {
                        validaParametros.resultErrors.ForEach(msgError => ModelState.AddModelError("", msgError));
                        return mod;
                    }
                }

            }

            #region Busca informacion dataset
            foreach (ModItem item in mod.Items)
            {
                foreach (ModGDataset ds in item.gDatasets)
                {
                    string query = this.strExecSP(ds.ProcedimientoAlm, mod.Parametros);
                    DataSet dsRes = ConsWsDatos.GetDataSet(query, log);
                    if (dsRes == null || dsRes.Tables.Count == 0)
                    {
                        log.Error($"Llamada sp {query} no genero ningun resultado.");
                        throw new HttpException(500, $"Llamada procedimiento almacenado no genero ningun resultado.");
                    }

                    ds.dtResProcedimiento = dsRes.Tables[0];

                    item.setLabelsFromDataTable(dsRes.Tables[0], ds.getLabelFromColumn);
                    ds.setGraficoDataFromDataTable(item.gLabels, dsRes.Tables[0], ds.dataOperation);
                }

                item.Grafico = new Grafico(item.gDatasets, item.gLabels);
            }
            #endregion

            return mod;
        }
        private string strExecSP(string sp, List<ModParametros> param)
        {
            string str = string.Empty;

            if(param.Count == 0)
                return $"EXEC {sp};";

            foreach (ModParametros par in param.OrderBy(x => x.Orden))
            {
                if (string.IsNullOrEmpty(par.Valor)) //si es null ocupa valor defecto
                    par.Valor = par.htmlValue;

                switch (par.Tipo)
                {
                    case AppEnums.ParamTipo.INT:
                        str += string.Format(",@{0} = {1}", par.NombreParSP, string.IsNullOrEmpty(par.Valor) ? "0" : par.Valor);
                        break;
                    case AppEnums.ParamTipo.DATE:
                        str += string.Format(",@{0} = '{1}'", par.NombreParSP, par.Valor.Replace("-", ""));
                        break;
                    case AppEnums.ParamTipo.STRING:
                    default:
                        str += string.Format(",@{0} = '{1}'", par.NombreParSP, par.Valor);
                        break;
                }
            }
            str = $"EXEC {sp} {str.Substring(1)};"; //substring es para quita primera "coma"
            return str;
        }
    }
}