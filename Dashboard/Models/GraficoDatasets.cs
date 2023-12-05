using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class GraficoDatasets
    {
        public string label { get; set; }
        public List<string> data { get; set; }
        //public List<string> backgroundColor { get; set; }
        public bool? fill { get; set; }
        //public List<string> borderColor { get; set; }
        //public int? borderWidth { get; set; }
        public double? tension { get; set; }

        public GraficoDatasets()
        {
            this.data = new List<string>();
        }
    }
}