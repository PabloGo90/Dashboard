using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dashboard.Models;

namespace Dashboard.Models
{
    public class Grafico
    {
        public List<string> labels { get; set; }
        public List<GraficoDatasets> datasets { get; set; }

        public Grafico()
        {
            this.labels = new List<string>();
            this.datasets = new List<GraficoDatasets>();
        }
        public Grafico(List<ModGDataset> _dataset, List<string> _labels)
        {
            GraficoLabelsData _dAux;
            this.labels = _labels;
            this.datasets = new List<GraficoDatasets>();

            foreach (ModGDataset ds in _dataset)
            {
                GraficoDatasets graficoDatasets = new GraficoDatasets() {
                    label = ds.DsLabelTitle,
                    data = new List<string>(),
                    //backgroundColor = new List<string> { "rgba(255, 99, 132, 0.2)" },
                    //borderColor = new List<string> { "rgb(255, 99, 132)" },
                    //borderWidth = 1,
                };

                foreach(string lbl in _labels)
                {
                    _dAux = ds.data.FirstOrDefault(x => x.label == lbl);
                    graficoDatasets.data.Add(_dAux != null ? _dAux.labelData : "0");
                }

                this.datasets.Add(graficoDatasets);
            }
        }
    }
}
