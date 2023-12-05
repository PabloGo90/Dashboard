using Dashboard.WsDatos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace Dashboard.Helpers
{
    public class ConsWsDatos
    {
        public static string GetJsonFromDataSet(String string_llamada_SP, Serilog.ILogger log = null, int dsTable = 0)
        {
            using (DataSet ds = GetDataSet(string_llamada_SP, log))
            {
                if (ds == null)
                    throw new Exception("Procedimiento almacenado no genero retorno");
                if (ds.Tables.Count < dsTable + 1)
                    throw new Exception($"Procedimiento almacenado no genero tabla {dsTable}");

                return JsonConvert.SerializeObject(ds.Tables[dsTable]);
            }
        }
        public static List<T> GetObjectFromDataSet<T>(String string_llamada_SP, Serilog.ILogger log = null, int dsTable = 0)
        {
            List<T> data = new List<T>();
            using (DataSet ds = GetDataSet(string_llamada_SP,log))
            {
                if (ds == null && ds.Tables.Count >= dsTable + 1)
                    return null;

                foreach (DataRow row in ds.Tables[dsTable].Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
                return data;
            }

        }
        public static string ExecuteScalar(String string_llamada_SP, Serilog.ILogger log= null, int dsTable = 0)
        {
            using (DataSet ds = GetDataSet(string_llamada_SP, log))
            {
                if (ds == null && ds.Tables.Count >= dsTable + 1)
                    return null;

                try
                {
                    return ds.Tables[dsTable].Rows[0][0].ToString();
                }
                catch { return string.Empty; }
            }

        }
        public static bool ExecuteNonQuery(String string_llamada_SP, Serilog.ILogger log = null)
        {
            if (log != null)
                log.Debug(string_llamada_SP);

            //Asegurarse de que se esta ejecutando sitio WsDatos con dll nombre wsConsultaDatosCarga.dll (si es otra generara error)
            using (ServicioDatosSoapClient _serviciodatos = new ServicioDatosSoapClient())
            {
                if (log != null)
                    log.Debug($"Consumiendo WS: {_serviciodatos.Endpoint.Address.ToString()}");

                _serviciodatos.EjecutaConsulta(System.Configuration.ConfigurationManager.AppSettings["ConexionBECH"], "", string_llamada_SP, "");
            }
            return true;
        }
        public static DataSet GetDataSet(String string_llamada_SP, Serilog.ILogger log)
        {
            if (log != null)
                log.Debug(string_llamada_SP);

            //Asegurarse de que se esta ejecutando sitio WsDatos con dll nombre wsConsultaDatosCarga.dll (si es otra generara error)
            using (ServicioDatosSoapClient _serviciodatos = new ServicioDatosSoapClient())
            {
                if (log != null)
                    log.Debug($"Consumiendo WS: {_serviciodatos.Endpoint.Address.ToString()}");

                return _serviciodatos.EjecutaConsulta(System.Configuration.ConfigurationManager.AppSettings["ConexionBECH"], "", string_llamada_SP, "");
            }
        }
        public static float TiempoUltimaAccionUsuario(string identity, string ip)
        {
            float TiempoUltimaAccion = 0;
            DataSet ds = new DataSet();
            try
            {
                String _query = "exec spSelDshVerficaUltimaConexion @identity = '" + identity + "', @ip_usuario = '" + ip + "'";
                ds = GetDataSet(_query, null);
                if (ds.Tables[0].Rows.Count > 0)
                    TiempoUltimaAccion = float.Parse(ds.Tables[0].Rows[0]["minutos"].ToString());
            }
            catch (Exception ex)
            {
                if (ex.Message.Length > 0)
                {
                    return TiempoUltimaAccion;
                }
            }
            finally
            {
                ds.Clear();
            }
            return TiempoUltimaAccion;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (dr[column.ColumnName].GetType() == pro.PropertyType)
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        else
                        {
                            if (pro.PropertyType == typeof(string))
                                pro.SetValue(obj, dr[column.ColumnName].ToString(), null);
                            else if (!string.IsNullOrEmpty(dr[column.ColumnName].ToString()))
                            {
                                if (pro.PropertyType == typeof(int))
                                    pro.SetValue(obj, int.Parse(dr[column.ColumnName].ToString()), null);
                                else if (pro.PropertyType == typeof(Int64))
                                    pro.SetValue(obj, Int64.Parse(dr[column.ColumnName].ToString()), null);
                                else if (pro.PropertyType == typeof(Int16))
                                    pro.SetValue(obj, Int16.Parse(dr[column.ColumnName].ToString()), null);
                                else if (pro.PropertyType == typeof(double))
                                    pro.SetValue(obj, double.Parse(dr[column.ColumnName].ToString()), null);
                                else if (pro.PropertyType == typeof(bool))
                                    pro.SetValue(obj, bool.Parse(dr[column.ColumnName].ToString()), null);
                                else if (pro.PropertyType == typeof(DateTime))
                                    pro.SetValue(obj, DateTime.Parse(dr[column.ColumnName].ToString()), null);
                            }
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        public static String obtenerip()
        {
            string ipaddress = "";

            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ipaddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();

            if (ipaddress == "" || ipaddress is null)
                ipaddress = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();

            if (ipaddress == "" || ipaddress is null)
                ipaddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

            return ipaddress;
        }

        public static bool ValidaCampo(string value, string loginusuario, string NomCampo, string StrRegex, out string MsjRes)
        {
            MsjRes = "";
            if (String.IsNullOrWhiteSpace(value.Trim()))
            {
                MsjRes = NomCampo + " vacío, ingrese datos.";
                return false;
            }
            if (StrRegex.Length > 0)
            {
                if (!Regex.IsMatch(value.Trim(), StrRegex))
                {
                    MsjRes = "Caracteres no válidos en " + NomCampo;
                    return false;
                }
            }
            if ((NomCampo == "Nombre") || (NomCampo == "Rut") || (NomCampo == "Mail"))
            {
                String _sqlSelect = "exec spSelDshValidaCamposUsuarios ,@loginusuario=" + loginusuario;

                if (NomCampo == "Nombre")
                    _sqlSelect += ", @nomusuario = '" + value.Trim() + "'";

                if (NomCampo == "Rut")
                {
                    if (!ValidaRut.EsRutValido(value.Trim()))
                    {
                        MsjRes = NomCampo + " invalido.";
                        return false;
                    }

                    _sqlSelect += ", @rutusuario = '" + value.Trim() + "'";
                }


                if (NomCampo == "Mail")
                    _sqlSelect += ", @email = '" + value.Trim() + "'";

                foreach (DataRow _DataRow in GetDataSet(_sqlSelect, null).Tables[0].Rows)
                    if (_DataRow["salida"].ToString() == "1")
                    {
                        MsjRes = NomCampo + " ya existe.";
                        return false;
                    }
            }

            return true;
        }
    }
}