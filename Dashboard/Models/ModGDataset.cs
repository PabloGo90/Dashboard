using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class ModGDataset
    {
        public int Id { get; set; }
        public int IdItem { get; set; }
        public string DsLabelTitle { get; set; }
        public string getLabelFromColumn { get; set; } //columna del sp en que se extraeran los labels del grafico (ejemplo user1, user2)
        public string getDataFromColumn { get; set; } //columna del sp en que se extraeran los datos del grafico (ejemplo 10, 20)
        public List<GraficoLabelsData> data { get; set; } //datos grafico
        public string ProcedimientoAlm { get; set; } // Nombre sp
        //public string jsonResProcedimiento { get; set; } // resultado del procedimiento almacenado en json
        public DataTable dtResProcedimiento { get; set; }// resultado del procedimiento almacenado en dt
        public AppEnums.DataOperation dataOperation { get; set; } //operacion aritmetica sobre la columna data (suma, promedio, conteo)

        public ModGDataset()
        {
            this.data = new List<GraficoLabelsData>();
            this.dtResProcedimiento = new DataTable();
        }
        public ModGDataset(DataRow row)
        {
            AppEnums.DataOperation aux;
            this.data = new List<GraficoLabelsData>();
            this.dtResProcedimiento = new DataTable();
            this.Id = int.Parse(row["id"].ToString());
            this.IdItem = int.Parse(row["idModuloDet"].ToString());
            this.DsLabelTitle = row["dsTitulo"].ToString();
            this.getDataFromColumn = row["getDataFromColumn"].ToString();
            this.getLabelFromColumn = row["getLabelFromColumn"].ToString();
            this.ProcedimientoAlm = row["nombreSP"].ToString();
            if (Enum.TryParse<AppEnums.DataOperation>(row["dataOperation"].ToString().ToUpper(), out aux))
                this.dataOperation = aux;
        }
        public void setGraficoDataFromDataTable(List<string> labels, DataTable dt, AppEnums.DataOperation operation)
        {
            double aux = 0;
            int gEntryCount = 0;
            foreach (string lbl in labels)
                this.data.Add(new GraficoLabelsData() { label = lbl });

            foreach (GraficoLabelsData gEntry in data)
            {
                gEntryCount = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (row[this.getLabelFromColumn].ToString() == gEntry.label)
                    {
                        switch (operation)
                        {
                            case AppEnums.DataOperation.COUNT:
                                gEntry._labelData ++;
                                break;
                            case AppEnums.DataOperation.AVG:
                                double.TryParse(row[this.getDataFromColumn].ToString(), out aux);
                                gEntry._labelData += aux;
                                gEntryCount ++;
                                break;
                            case AppEnums.DataOperation.SUM:
                            default:
                                double.TryParse(row[this.getDataFromColumn].ToString(), out aux);
                                gEntry._labelData += aux;
                                break;
                        }
                    }
                }
                //Si es promedio, divide por total de operaciones
                if(operation == AppEnums.DataOperation.AVG)
                    gEntry._labelData = gEntry._labelData / gEntryCount;
            }
        }
    }
}