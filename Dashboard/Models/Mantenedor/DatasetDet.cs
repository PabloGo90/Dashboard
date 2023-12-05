using System;

namespace Dashboard.Models.Mantenedor
{

    public class DatasetDet
    {
        private string _dataOperation;

        public string dsTitulo { get; set; } = string.Empty;
        public string nombreSP { get; set; } = string.Empty;
        public string getLabelFromColumn { get; set; } = string.Empty;
        public string getDataFromColumn { get; set; } = string.Empty;
        public string dataOperation  { get { return (this._dataOperation ?? "").ToUpper(); } set { this._dataOperation = value; } }

        public void valida()
        {

            AppEnums.DataOperation aux;

            if (string.IsNullOrEmpty(this.dsTitulo) || string.IsNullOrEmpty(this.dsTitulo.Trim()))
                throw new System.Exception("detalle.dataset.dsTitulo no puede ser vacio");
            if (string.IsNullOrEmpty(this.nombreSP) || string.IsNullOrEmpty(this.nombreSP.Trim()))
                throw new System.Exception("detalle.dataset.nombreSP no puede ser vacio");
            if (string.IsNullOrEmpty(this.getLabelFromColumn) || string.IsNullOrEmpty(this.getLabelFromColumn.Trim()))
                throw new System.Exception("detalle.dataset.getLabelFromColumn no puede ser vacio");
            if (string.IsNullOrEmpty(this.getDataFromColumn) || string.IsNullOrEmpty(this.getDataFromColumn.Trim()))
                throw new System.Exception("detalle.dataset.getDataFromColumn no puede ser vacio");

            if (!Enum.TryParse(this.dataOperation, out aux))
                throw new System.Exception($"detalle.dataset.dataoperation no es valido. valores soportados {String.Join(", ", Enum.GetNames(typeof(AppEnums.DataOperation)))}");
            
        }
    }
}