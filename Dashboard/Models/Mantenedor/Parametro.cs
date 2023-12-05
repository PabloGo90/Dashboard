using System;

namespace Dashboard.Models.Mantenedor
{
    public class Parametro
    {
        private string _valorMin;
        private string _valorMax;
        private string _valorDefecto;
        private string _tipo;
        private string _tipoHtml;
        private string _validacion;
        private string _validacionCondicion;

        public string nombreParSP { get; set; } = string.Empty;
        public string nombreParFiltro { get; set; } = string.Empty;
        public string valorDefecto { get { return (this._valorDefecto ?? "").ToUpper(); } set { this._valorDefecto = value; } }
        public int obligatorio { get; set; }
        public string tipo { get { return (this._tipo ?? "").ToUpper(); } set { this._tipo = value; } }
        public string tipoHtml { get { return (this._tipoHtml ?? "").ToUpper(); } set { this._tipoHtml = value; } }
        public string selectListSPData { get; set; } = string.Empty;
        public int orden { get; set; }
        public int largoMax { get; set; }
        public string validacion { get { return (this._validacion ?? "").ToUpper(); } set { this._validacion = value; } }
        public string validacionCondicion { get { return (this._validacionCondicion ?? "").ToUpper(); } set { this._validacionCondicion = value; } }
        public string validacionCondicionValor { get; set; } = string.Empty;
        public int largoMin { get; set; }
        public string valorMin { get { return (this._valorMin ?? "").ToUpper(); } set { this._valorMin = value; } }
        public string valorMax { get { return (this._valorMax ?? "").ToUpper(); } set { this._valorMax = value; } }



        public void valida()
        {
            AppEnums.ParamTipo _tipo;
            AppEnums.ParamTipoHtml _tipoHtml;
            AppEnums.ParamValorDefecto _valorDefecto;
            AppEnums.ParamValidacion _validacion;
            AppEnums.ParamValidacionCondicion _validacionCondicion;
            AppEnums.ParamValorMin _valorMin;
            AppEnums.ParamValorMax _valorMax;
            int n = 0;

            if (string.IsNullOrEmpty(this.nombreParSP) || string.IsNullOrEmpty(this.nombreParSP.Trim()))
                throw new System.Exception("nombreParSP no puede ser vacio");
            if (string.IsNullOrEmpty(this.nombreParFiltro) || string.IsNullOrEmpty(this.nombreParFiltro.Trim()))
                throw new System.Exception("nombreParFiltro no puede ser vacio");

            if (int.TryParse(this.tipo, out n) || !Enum.TryParse(this.tipo, out _tipo))
                throw new System.Exception($"parametro.tipo no es valido. valores soportados {String.Join(", ", Enum.GetNames(typeof(AppEnums.ParamTipo)))}");

            if (int.TryParse(this.tipoHtml, out n) || !Enum.TryParse(this.tipoHtml, out _tipoHtml))
                throw new System.Exception($"parametro.tipo html no es valido. valores soportados {String.Join(", ", Enum.GetNames(typeof(AppEnums.ParamTipoHtml)))}");

            if (!string.IsNullOrEmpty(this.valorDefecto))
                if (int.TryParse(this.valorDefecto, out n) || !Enum.TryParse(this.valorDefecto, out _valorDefecto))
                    throw new System.Exception($"parametro.valordefecto no es valido. valores soportados {String.Join(", ", Enum.GetNames(typeof(AppEnums.ParamValorDefecto)))}");

            if (!string.IsNullOrEmpty(this.validacion))
                if (int.TryParse(this.validacion, out n) || !Enum.TryParse(this.validacion, out _validacion))
                    throw new System.Exception($"parametro.validacion no es valido. valores soportados {String.Join(", ", Enum.GetNames(typeof(AppEnums.ParamValidacion)))}");

            if (!string.IsNullOrEmpty(this.validacionCondicion))
                if (int.TryParse(this.validacionCondicion, out n) || !Enum.TryParse(this.validacionCondicion, out _validacionCondicion))
                    throw new System.Exception($"parametro.validacioncondicion no es valido. valores soportados {String.Join(", ", Enum.GetNames(typeof(AppEnums.ParamValidacionCondicion)))}");

            if (!string.IsNullOrEmpty(this.valorMin))
                if (int.TryParse(this.valorMin, out n) || !Enum.TryParse(this.valorMin, out _valorMin))
                    throw new System.Exception($"parametro.valormin no es valido. valores soportados {String.Join(", ", Enum.GetNames(typeof(AppEnums.ParamValorMin)))}");

            if (!string.IsNullOrEmpty(this.valorMax))
                if (int.TryParse(this.valorMax, out n) || !Enum.TryParse(this.valorMax, out _valorMax))
                    throw new System.Exception($"parametro.valormax no es valido. valores soportados {String.Join(", ", Enum.GetNames(typeof(AppEnums.ParamValorMax)))}");

            if (this.obligatorio > 1 || this.obligatorio < 0)
                throw new System.Exception($"parametro.obligatorio no es valido. valores soportados 0, 1");

        }
    }
}