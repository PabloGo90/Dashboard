using System.Collections.Generic;
using System.Linq;

namespace Dashboard.Models.Mantenedor
{

    public class DashboardObj
    {
        string _activo;

        //public int id { get; set; }
        public string nombre { get; set; }
        public string nombreAgrupadorMenu { get; set; }
        public string descripcion { get; set; }
        public string nombreMenu { get; set; }
        public string nombreIconoMenu { get; set; }
        public int orden { get; set; }
        public string activo { get { return (this._activo ?? "").ToUpper(); } set { this._activo = value; } }
        public List<Parametro> parametros { get; set; }
        public List<Detalle> detalles { get; set; }

        public DashboardObj()
        {
            //this.id = 0;
            this.nombre = "";
            this.nombreAgrupadorMenu = "";
            this.descripcion = "";
            this.nombreMenu = "";
            this.nombreIconoMenu = "";
            this.activo = "Y";
            this.parametros = new List<Parametro>();
            this.detalles = new List<Detalle>();
        }
        public DashboardObj(bool test)
        {
            this.nombre = "";
            this.nombreAgrupadorMenu = "";
            this.descripcion = "";
            this.nombreMenu = "";
            this.nombreIconoMenu = "";
            this.activo = "Y";
            this.parametros = new List<Parametro>();
            this.detalles = new List<Detalle>();
            if (test)
                this.parametros.Add(new Parametro());
            if (test)
            {
                this.detalles.Add(new Detalle() { dataset = new List<DatasetDet>()});
                this.detalles.First().dataset = new List<DatasetDet>();
                this.detalles.First().dataset.Add(new DatasetDet());
            }
        }

        public void valida()
        {
            if (string.IsNullOrEmpty(this.nombre) || string.IsNullOrEmpty(this.nombre.Trim()))
                throw new System.Exception("nombre no puede ser vacio");
            if (string.IsNullOrEmpty(this.nombreMenu) || string.IsNullOrEmpty(this.nombreMenu.Trim()))
                throw new System.Exception("nombreMenu no puede ser vacio");
            if (this.activo != "Y" && this.activo != "N")
                throw new System.Exception("valor activo no es valido. valores soportados Y, N");

            if (this.parametros != null && this.parametros.Count > 0)
            {
                this.parametros.ForEach(p => p.valida());
            }

            if (this.detalles != null && this.detalles.Count > 0)
            {                
                foreach(Detalle det in this.detalles)
                {
                    if(this.detalles.Count(x => x.nombreGrafico == det.nombreGrafico) > 1)
                        throw new System.Exception($"Ya existe otro grafico con nombre {det.nombreGrafico}, no puede repetirse!");

                    det.valida();
                }
            }
        }
    }
}