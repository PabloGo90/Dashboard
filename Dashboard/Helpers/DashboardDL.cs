using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dashboard.Models.Mantenedor;

namespace Dashboard.Helpers
{
    public class DashboardDL : IDisposable
    {
        public DashboardObj GetDashboard(string nombre, Serilog.ILogger log, out int idFound)
        {
            DashboardObj dsh = new DashboardObj();
            int id = this.SelectIDModulo(nombre, log);
            if (id <= 0)
                throw new Exception("Dashboard no encontrado!");

            dsh = ConsWsDatos.GetObjectFromDataSet<DashboardObj>($"exec spSelDshModulo {id}", log).FirstOrDefault();

            if(dsh == null)
                throw new Exception("Dashboard no es posible cargar!");
            
            idFound = id;//dsh.id = id;
            dsh.parametros = ConsWsDatos.GetObjectFromDataSet<Parametro>($"exec spSelDshParametroModulo {id}", log);
            dsh.detalles = ConsWsDatos.GetObjectFromDataSet<Detalle>($"exec spSelDshModuloDetalle {id}", log);
            
            foreach(Detalle d in dsh.detalles)
            {
                d.dataset = ConsWsDatos.GetObjectFromDataSet<DatasetDet>($"exec spSelDshModuloDetalleDS {d.id}", log);
            }

            return dsh;
        }
        public int SelectIDModulo(string nombreModulo, Serilog.ILogger log)
        {
            int aux;
            int.TryParse(ConsWsDatos.ExecuteScalar($"exec spSelDshIdModulo  '{nombreModulo}'", log), out aux);
            return aux;
        }
        public int SelectIDModuloDetalle(string nombreDetalle, int idModulo, Serilog.ILogger log)
        {
            int aux;
            int.TryParse(ConsWsDatos.ExecuteScalar($"exec spSelDshIDModuloDetalle  {idModulo}, '{nombreDetalle}'", log), out aux);
            return aux;
        }
        public void DeleteModulo(int id, Serilog.ILogger log)
        {
            if (!ConsWsDatos.ExecuteNonQuery($"exec spDelDshElimModulo {id}", log))
                throw new Exception("Error al eliminar modulo");
        }
        public bool InsertModulo(DashboardObj dsh, Serilog.ILogger log)
        {

            if (!ConsWsDatos.ExecuteNonQuery($"exec spInsDshNuevoModulo  " +
                                            $"'{dsh.nombreAgrupadorMenu}','{dsh.nombre}', '{dsh.descripcion}','{dsh.nombreMenu}', '{dsh.nombreIconoMenu}', '{dsh.activo}', {dsh.orden}", log))
                throw new Exception("Error al insertar modulo");

            return true;
        }
        public bool UpdateModulo(int id, DashboardObj dsh, Serilog.ILogger log)
        {
            if (!ConsWsDatos.ExecuteNonQuery($"exec spUpdDshActualizaModulo  " +
                                            $"{id},'{dsh.nombre}','{dsh.descripcion}', '{dsh.nombreMenu}', '{dsh.nombreIconoMenu}', " +
                                            $"'{dsh.nombreAgrupadorMenu}', {dsh.orden}, '{dsh.activo}'", log))
                throw new Exception("Error al actualizar modulo");

            return true;
        }
        public void DeleteDetails(int id, Serilog.ILogger log)
        {
            if (!ConsWsDatos.ExecuteNonQuery($"exec spDelDshElimDetalleModulo {id}", log))
                throw new Exception("Error al eliminar modulo");
        }        
        public bool InsertParametros(int id, List<Parametro> parametros, Serilog.ILogger log)
        {
            foreach (Parametro p in parametros)
            {
                if (!ConsWsDatos.ExecuteNonQuery($"exec spInsDshNuevoParametroModulo  " +
                                            $" {id}, '{p.nombreParSP}', '{p.nombreParFiltro}', '{p.valorDefecto}', {p.obligatorio}, '{p.tipo}','{p.tipoHtml}', '{p.selectListSPData}',{p.orden}, " +
                                            $"{p.largoMax}, '{p.validacion}', '{p.validacionCondicion}', '{p.validacionCondicionValor}', {p.largoMin}, '{p.valorMin}', '{p.valorMax}'", log))
                {
                    //rollback?
                    throw new Exception("Error al insertar parametros");
                }
            }

            return true;
        }
        public bool InsertDetalle(int id, List<Detalle> detalles, Serilog.ILogger log)
        {
            foreach (Detalle d in detalles)
            {
                if (!ConsWsDatos.ExecuteNonQuery($"exec spInsDshNuevoModuloDetalle  " +
                                             $"{id}, '{d.nombreGrafico}', '{d.nombreCorto}','{d.tipoGrafico}'", log))
                {
                    //rollback?
                    throw new Exception("Error al insertar detalle");
                }
            }

            return true;
        }
        public bool InsertDetalleDataset(int idMod, int idDet, List<DatasetDet> dataset, Serilog.ILogger log)
        {
            foreach (DatasetDet d in dataset)
            {
                if (!ConsWsDatos.ExecuteNonQuery($"exec spInsDshNuevoModuloDetalleDS  " +
                                             $"{idMod}, {idDet},'{d.dsTitulo}', '{d.nombreSP}', '{d.getLabelFromColumn}', '{d.getDataFromColumn}','{d.dataOperation}'", log))
                {
                    //rollback?
                    throw new Exception("Error al insertar detalle dataset");
                }
            }
            
            return true;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DashboardDL() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}