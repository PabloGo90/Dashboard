using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class ModItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
        public AppEnums.TipoGrafico TipoGrafico { get; set; }
        public Grafico Grafico { get; set; }
        public List<string> gLabels { get; set; }
        public List<ModGDataset> gDatasets { get; set; }
        public string html_ScrollIndex { get { return string.Format("scrollspyHeading{0}", this.Id); } }
        public string html_TipoGrafico { get { return this.TipoGrafico.ToString(); } }


        public ModItem()
        {
            this.Grafico = new Grafico();
            this.gDatasets = new List<ModGDataset>();
            this.gLabels = new List<string>();
        }
        public ModItem(DataRow row, List<ModGDataset> _gDataset)
        {
            AppEnums.TipoGrafico aux;
            this.Grafico = new Grafico();
            this.gDatasets = new List<ModGDataset>();
            this.gLabels = new List<string>();

            this.Id = int.Parse(row["id"].ToString());
            this.Nombre = row["nombreGrafico"].ToString();
            this.NombreCorto = row["nombreCorto"].ToString();
            if (Enum.TryParse<AppEnums.TipoGrafico>(row["tipoGrafico"].ToString().ToUpper(), out aux))
                this.TipoGrafico = aux;
            this.gDatasets = _gDataset.Where(y => y.IdItem == this.Id).ToList();
        }

        public void setLabelsFromDataTable(DataTable dt, string getLabelFromColumn)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (!this.gLabels.Exists(x => x == row[getLabelFromColumn].ToString()))
                    this.gLabels.Add(row[getLabelFromColumn].ToString());
            }
        }
    }
}