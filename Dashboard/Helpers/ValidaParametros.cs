using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Dashboard.Helpers
{
    public class ValidaParametros : IDisposable
    {
        public bool result { get; private set; }
        public List<string> resultErrors { get; private set; }
        private List<ModParametros> parametros;
        private DateTime dateAux;
        private DateTime dateAux2;
        private int intAux;
        private int intAux2;
        private Serilog.ILogger log;

        public ValidaParametros(Serilog.ILogger _log)
        {
            this.parametros = new List<ModParametros>();
            this.resultErrors = new List<string>();
            this.dateAux = new DateTime();
            this.dateAux2 = new DateTime();
            this.intAux = 0;
            this.intAux2 = 0;
            this.log = _log;
        }
        public ValidaParametros(Serilog.ILogger _log, List<ModParametros> _parametros)
        {
            this.parametros = _parametros;
            this.resultErrors = new List<string>();
            this.dateAux = new DateTime();
            this.dateAux2 = new DateTime();
            this.intAux = 0;
            this.intAux2 = 0;
            this.log = _log;
        }

        public bool Validar()
        {
            this.result = true;

            this.ValidaLargo();
            this.ValidaCamposObligatorios();
            this.ValidaFechas();
            this.ValidaParamRut();
            this.ValidaNumeros();
            this.ValidaValorMinimo();
            this.ValidaValorMaximo();

            return this.result;
        }

        private void ValidaLargo()
        {
            foreach (ModParametros par in parametros.Where(x => !string.IsNullOrEmpty(x.Valor) && (x.largoMin > 0 || x.largoMax > 0)))
            {
                if (par.largoMin != 0 && par.Valor.Length < par.largoMin)
                {
                    resultErrors.Add($"Campo {par.NombreParFiltro} debe ser mayor a {par.largoMin}");
                    result = false;
                }
                if (par.largoMax != 0 && par.Valor.Length > par.largoMax)
                {
                    resultErrors.Add($"Campo {par.NombreParFiltro} debe ser menor a {par.largoMax}");
                    result = false;
                }
            }
        }
        private void ValidaCamposObligatorios()
        {
            foreach (ModParametros par in parametros.Where(x => x.Obligatorio))
            {
                switch (par.TipoHtml)
                {
                    case AppEnums.ParamTipoHtml.TEXT:
                        if (string.IsNullOrEmpty(par.Valor))
                        {
                            resultErrors.Add($"Valor de {par.NombreParFiltro} no puede ser vacio.");
                            result = false;
                        }
                        break;
                    case AppEnums.ParamTipoHtml.DATE:
                        if (string.IsNullOrEmpty(par.Valor))
                        {
                            resultErrors.Add($"Debe indicar fecha para {par.NombreParFiltro}.");
                            result = false;
                        }
                        break;
                    case AppEnums.ParamTipoHtml.SELECT:
                        if (string.IsNullOrEmpty(par.Valor) || par.Valor == "-1")
                        {
                            resultErrors.Add($"Debe seleccionar opcion para {par.NombreParFiltro}.");
                            result = false;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private void ValidaFechas()
        {
            foreach (ModParametros par in parametros.Where(x => x.validacion == AppEnums.ParamValidacion.FECHA))
            {
                if (!DateTime.TryParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateAux))
                {
                    resultErrors.Add($"Fecha ingresada no valida en {par.NombreParFiltro}.");
                    return;
                }

                try
                {
                    var paramAComparar = parametros.FirstOrDefault(x => x.NombreParFiltro == par.validacionCondicionValor);
                    if (paramAComparar == null || !DateTime.TryParseExact(paramAComparar.Valor.Replace("-", ""),
                                                                            "yyyyMMdd", CultureInfo.InvariantCulture,
                                                                            DateTimeStyles.None, out dateAux2))
                    {
                        resultErrors.Add($"Error fecha a comparar {(paramAComparar == null ? "(no se encontro parametro)" : paramAComparar.NombreParFiltro)}.");
                        return;
                    }

                    switch (par.validacionCondicion)
                    {
                        case AppEnums.ParamValidacionCondicion.MAYOR:
                            if (!(dateAux > dateAux2))
                            {
                                resultErrors.Add($"Fecha {par.NombreParFiltro} debe ser mayor a {paramAComparar.NombreParFiltro}");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValidacionCondicion.MAYOROIGUAL:
                            if (!(dateAux >= dateAux2))
                            {
                                resultErrors.Add($"Fecha {par.NombreParFiltro} debe ser mayor o igual a {paramAComparar.NombreParFiltro}");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValidacionCondicion.MENOR:
                            if (!(dateAux < dateAux2))
                            {
                                resultErrors.Add($"Fecha {par.NombreParFiltro} debe ser menor a {paramAComparar.NombreParFiltro}");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValidacionCondicion.MENOROIGUAL:
                            if (!(dateAux <= dateAux2))
                            {
                                resultErrors.Add($"Fecha {par.NombreParFiltro} debe ser menor o igual a {paramAComparar.NombreParFiltro}");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValidacionCondicion.NINGUNA:
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Error al validar fechas, error de configuracion {ex.ToString()}");
                    resultErrors.Add("No se pudo validar fechas, error de configuracion.");
                    result = false;
                }
            }
        }
        private void ValidaParamRut()
        {
            foreach (ModParametros par in parametros.Where(x => x.validacion == AppEnums.ParamValidacion.RUT))
            {
                try
                {
                    if (string.IsNullOrEmpty(par.Valor))
                    {
                        resultErrors.Add($"Campo {par.NombreParFiltro} vacio");
                        result = false;
                    }
                    if (par.Valor.Contains("-"))
                    {
                        if (!ValidaRut.EsRutValido(par.Valor.Replace(".", "")))
                        {
                            resultErrors.Add($"Campo {par.NombreParFiltro} no es un Rut valido");
                            result = false;
                        }
                    }
                    else
                    {
                        string rut = par.Valor.Substring(0, par.Valor.Length - 1);
                        string dv = par.Valor.Substring(par.Valor.Length - 1, 1);
                        if (!ValidaRut.EsRutValido(rut.Replace(".", ""), dv))
                        {
                            resultErrors.Add($"Campo {par.NombreParFiltro} no es un Rut valido");
                            result = false;
                        }
                    }
                }
                catch(Exception ex)
                {
                    resultErrors.Add($"Error al validar Campo {par.NombreParFiltro}.");
                    result = false;
                    log.Error(ex.ToString());
                }
            }
        }
        private void ValidaNumeros()
        {
            foreach (ModParametros par in parametros.Where(x => x.validacion == AppEnums.ParamValidacion.NUMERO))
            {
                if (!int.TryParse(par.Valor, out intAux))
                {
                    resultErrors.Add($"Error {par.NombreParFiltro} no es numérico.");
                    return;
                }
                try
                {
                    var paramAComparar = parametros.FirstOrDefault(x => x.NombreParFiltro == par.validacionCondicionValor);
                    if (paramAComparar == null || !int.TryParse(paramAComparar.Valor, out intAux2))
                    {
                        resultErrors.Add($"Error numero a comparar {(paramAComparar == null ? "(no se encontro parametro)" : paramAComparar.NombreParFiltro)}.");
                        return;
                    }

                    switch (par.validacionCondicion)
                    {
                        case AppEnums.ParamValidacionCondicion.MAYOR:
                            if (!(intAux > intAux2))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser mayor a {paramAComparar.NombreParFiltro}");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValidacionCondicion.MAYOROIGUAL:
                            if (!(intAux >= intAux2))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser mayor o igual a {paramAComparar.NombreParFiltro}");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValidacionCondicion.MENOR:
                            if (!(intAux < intAux2))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser menor a {paramAComparar.NombreParFiltro}");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValidacionCondicion.MENOROIGUAL:
                            if (!(intAux <= intAux2))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser menor o igual a {paramAComparar.NombreParFiltro}");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValidacionCondicion.NINGUNA:
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Error al validar fechas, error de configuracion {ex.ToString()}");
                    resultErrors.Add("No se pudo validar fechas, error de configuracion.");
                    result = false;
                }
            }
        }
        private void ValidaValorMinimo()
        {
            foreach (ModParametros par in parametros.Where(x => x.valorMin != AppEnums.ParamValorMin.NINGUNA))
            {
                try
                {
                    switch (par.valorMin)
                    {
                        case AppEnums.ParamValorMin.ZERO:
                            if (int.Parse(par.Valor) < 0)
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser mayor a 0");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMin.LASTWEEK:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) <= DateTime.Now.AddDays(-7))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser mayor a una semana");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMin.LASTMONTH:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) <= DateTime.Now.AddMonths(-1))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser mayor a un mes");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMin.LASTYEAR:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) <= DateTime.Now.AddYears(-1))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser mayor a un año");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMin.LAST2YEAR:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) <= DateTime.Now.AddYears(-2))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser mayor a 2 años");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMin.LAST3YEAR:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) <= DateTime.Now.AddYears(-3))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser mayor a 3 años");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMin.LAST4YEAR:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) <= DateTime.Now.AddYears(-4))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser mayor a 4 años");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMin.LAST5YEAR:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) <= DateTime.Now.AddYears(-5))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser mayor a 5 años");
                                result = false;
                            }
                            break;
                        default:
                            break;
                    }

                }
                catch (Exception ex)
                {
                    log.Error($"Error al validar valor minimo, error de configuracion {ex.ToString()}");
                    resultErrors.Add("No se pudo validar valor minimo, error de configuracion.");
                    result = false;
                }
            }
        }
        private void ValidaValorMaximo()
        {
            foreach (ModParametros par in parametros.Where(x => x.valorMax != AppEnums.ParamValorMax.NINGUNA))
            {
                try
                {
                    switch (par.valorMax)
                    {
                        case AppEnums.ParamValorMax.NOW:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) >= DateTime.Now)
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser menor o igual a hoy");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMax.NEXTWEEK:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) >= DateTime.Now.AddDays(7))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser menor a una semana");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMax.NEXTMONTH:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) >= DateTime.Now.AddMonths(1))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser menor a un mes");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMax.NEXTYEAR:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) >= DateTime.Now.AddYears(1))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser menor a un año");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMax.NEXT2YEAR:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) >= DateTime.Now.AddYears(2))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser menor a 2 años");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMax.NEXT3YEAR:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) >= DateTime.Now.AddYears(3))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser menor a 3 años");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMax.NEXT4YEAR:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) >= DateTime.Now.AddYears(4))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser menor a 4 años");
                                result = false;
                            }
                            break;
                        case AppEnums.ParamValorMax.NEXT5YEAR:
                            if (DateTime.ParseExact(par.Valor.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture) >= DateTime.Now.AddYears(5))
                            {
                                resultErrors.Add($"{par.NombreParFiltro} debe ser menor a 5 años");
                                result = false;
                            }
                            break;
                        default:
                            break;
                    }

                }
                catch (Exception ex)
                {
                    log.Error($"Error al validar valor minimo, error de configuracion {ex.ToString()}");
                    resultErrors.Add("No se pudo validar valor minimo, error de configuracion.");
                    result = false;
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    resultErrors.Clear();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ValidaParametros() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}