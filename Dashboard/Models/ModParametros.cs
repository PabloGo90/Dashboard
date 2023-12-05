using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class ModParametros
    {
        public int Id { get; set; }
        public int IdModulo { get; set; }
        public string NombreParSP { get; set; }
        public string NombreParFiltro { get; set; }
        public AppEnums.ParamValorDefecto ValorDefecto { get; set; }
        public string Valor { get; set; }
        public AppEnums.ParamTipo Tipo { get; set; }
        public AppEnums.ParamTipoHtml TipoHtml { get; set; }
        public string selectListSPData { get; set; } //valido solo para ParamTipoHtml.select
        public List<ComboBox> comboBox_niv1 { get; set; }
        public List<ComboBox> comboBox_niv2 { get; set; }
        public List<ComboBox> comboBox_niv3 { get; set; }
        public bool Obligatorio { get; set; }
        public int largoMax { get; set; }
        public int largoMin { get; set; } //todo
        public AppEnums.ParamValorMin valorMin { get; set; }
        public AppEnums.ParamValorMax valorMax { get; set; }
        public AppEnums.ParamValidacion validacion { get; set; }
        public AppEnums.ParamValidacionCondicion validacionCondicion { get; set; }
        public string validacionCondicionValor { get; set; }
        public int Orden { get; set; }

        #region Gets html
        public string htmlId
        {
            get
            {
                return string.Format("param_{0}", this.Id);
            }
        }
        public string htmlName { get { return this.NombreParSP; } }
        public string htmlType { get { return this.TipoHtml.ToString().ToLower(); } }
        public string htmlValue
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Valor))
                    return this.Valor;

                switch (this.ValorDefecto)
                {
                    case AppEnums.ParamValorDefecto.ZERO:
                        return "0";
                    case AppEnums.ParamValorDefecto.LASTMONTH:
                        return DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    case AppEnums.ParamValorDefecto.LASTWEEK:
                        return DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                    case AppEnums.ParamValorDefecto.NOW:
                        return DateTime.Now.ToString("yyyy-MM-dd");
                    default:
                        return "";
                }
            }
        }
        #endregion

        public ModParametros()
        {
            this.comboBox_niv1 = new List<ComboBox>();
            this.comboBox_niv2 = new List<ComboBox>();
            this.comboBox_niv3 = new List<ComboBox>();
        }
        public ModParametros(DataRow row)
        {
            AppEnums.ParamTipo _tipo;
            AppEnums.ParamTipoHtml _tipoHtml;
            AppEnums.ParamValorDefecto _valorDefecto;
            AppEnums.ParamValidacion _validacion;
            AppEnums.ParamValidacionCondicion _validacionCondicion;
            AppEnums.ParamValorMin _valorMin;
            AppEnums.ParamValorMax _valorMax;

            this.comboBox_niv1 = new List<ComboBox>();
            this.comboBox_niv2 = new List<ComboBox>();
            this.comboBox_niv3 = new List<ComboBox>();
            this.Id = int.Parse(row["id"].ToString());
            this.NombreParSP = row["nombreParSP"].ToString();
            this.NombreParFiltro = row["nombreParFiltro"].ToString();
            this.Orden = int.Parse(string.IsNullOrEmpty(row["orden"].ToString()) ? "0" : row["orden"].ToString());
            this.Obligatorio = row["obligatorio"].ToString() == "1";
            this.largoMin = int.Parse(string.IsNullOrEmpty(row["largoMin"].ToString()) ? "0" : row["largoMin"].ToString());
            this.largoMax = int.Parse(string.IsNullOrEmpty(row["largoMax"].ToString()) ? "0" : row["largoMax"].ToString());
            this.validacionCondicionValor = row["validacionCondicionValor"].ToString();
            this.selectListSPData = row["selectListSPData"].ToString();
            if (Enum.TryParse(row["tipo"].ToString().ToUpper(), out _tipo))
                this.Tipo = _tipo;
            if (Enum.TryParse(row["valorDefecto"].ToString().ToUpper(), out _valorDefecto))
                this.ValorDefecto = _valorDefecto;
            else
                this.ValorDefecto = this.Tipo == AppEnums.ParamTipo.INT ? AppEnums.ParamValorDefecto.ZERO : AppEnums.ParamValorDefecto.EMPTY;
            if (Enum.TryParse(row["validacion"].ToString().ToUpper(), out _validacion))
                this.validacion = _validacion;
            if (Enum.TryParse(row["validacionCondicion"].ToString().ToUpper(), out _validacionCondicion))
                this.validacionCondicion = _validacionCondicion;
            if (Enum.TryParse(row["valorMin"].ToString().ToUpper(), out _valorMin))
                this.valorMin = _valorMin;
            if (Enum.TryParse(row["valorMax"].ToString().ToUpper(), out _valorMax))
                this.valorMax = _valorMax;
            if (Enum.TryParse<AppEnums.ParamTipoHtml>(row["tipoHtml"].ToString().ToUpper(), out _tipoHtml))
                this.TipoHtml = _tipoHtml;
        }

        public void setModParametrosCombos(DataSet ds)
        {
            ComboBox _aux = new ComboBox();
            //Dataset
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                _aux = new ComboBox(row, 1);
                if (!this.comboBox_niv1.Exists(x => x.value == _aux.value))
                    comboBox_niv1.Add(_aux);

                _aux = new ComboBox(row, 2);
                if (!string.IsNullOrEmpty(_aux.name) && !this.comboBox_niv2.Exists(x => x.name == _aux.name))
                    comboBox_niv2.Add(_aux);

                _aux = new ComboBox(row, 3);
                if (!string.IsNullOrEmpty(_aux.name) && !this.comboBox_niv3.Exists(x => x.name == _aux.name))
                    comboBox_niv3.Add(_aux);
            }
        }
        public void setSelectedParametrosCombos()
        {
            var selected = this.comboBox_niv1.FirstOrDefault(y => y.value == this.Valor) ?? new ComboBox();
            selected.selected = true;
            if (this.comboBox_niv2.Count > 0)
            {
                var selected2 = this.comboBox_niv2.FirstOrDefault(y => y.value == selected.value) ?? new ComboBox();
                selected2.selected = true;
                if (this.comboBox_niv3.Count > 0)
                {
                    var selected3 = this.comboBox_niv2.FirstOrDefault(y => y.value == selected2.value) ?? new ComboBox();
                    selected3.selected = true;
                }
            }
        }
    }
}