using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace Dashboard.Models
{
    public class GraficoLabelsData
    {
        public string label { get; set; }
        public string labelData { get { return this._labelData.ToString().Replace(",", "."); } }
        public double _labelData { get; set; }
    }
}