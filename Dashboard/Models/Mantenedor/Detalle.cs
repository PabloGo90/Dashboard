using System;
using System.Collections.Generic;

namespace Dashboard.Models.Mantenedor
{
    public class Detalle
    {
        private string _tipoGrafico;

        public int id { get; set; }
        public string nombreGrafico { get; set; } = string.Empty;
        public string nombreCorto { get; set; } = string.Empty;
        public string tipoGrafico { get { return (this._tipoGrafico ?? "").ToUpper(); } set { this._tipoGrafico = value; } }
        public List<DatasetDet> dataset { get; set; } = new List<DatasetDet>();

        public void valida()
        {
            AppEnums.TipoGrafico aux;

            if (string.IsNullOrEmpty(this.nombreGrafico) || string.IsNullOrEmpty(this.nombreGrafico.Trim()))
                throw new System.Exception("detalle.nombreGrafico no puede ser vacio");
            if (string.IsNullOrEmpty(this.nombreCorto) || string.IsNullOrEmpty(this.nombreCorto.Trim()))
                throw new System.Exception("detalle.nombreCorto no puede ser vacio");
            if (string.IsNullOrEmpty(this.tipoGrafico) || string.IsNullOrEmpty(this.tipoGrafico.Trim()))
                throw new System.Exception("detalle.tipoGrafico no puede ser vacio");

            if (!Enum.TryParse(this.tipoGrafico, out aux))
                throw new System.Exception($"detalle.tipoGrafico no es valido. valores soportados {String.Join(", ", Enum.GetNames(typeof(AppEnums.TipoGrafico)))}");
            
            if (this.dataset == null || this.dataset.Count == 0)
                throw new System.Exception("detalle.Grafico debe contener dataset");
            else
                this.dataset.ForEach(d => d.valida());
        }
    }
}