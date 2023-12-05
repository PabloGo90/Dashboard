using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class ModHome : ModMenu
    {
        public string Descripcion { get; set; }
        public string TipoGraficoStr { get; set; }
        public AppEnums.TipoGrafico TipoGrafico
        {
            get
            {
                AppEnums.TipoGrafico aux;
                Enum.TryParse<AppEnums.TipoGrafico>(this.TipoGraficoStr.ToUpper(), out aux);
                return aux;
            }
        }
        public string Img
        {
            get
            {
                switch (this.TipoGrafico)
                {
                    case AppEnums.TipoGrafico.BAR:
                        return "bar-chart.png";
                    case AppEnums.TipoGrafico.DOUGHNUT:
                    case AppEnums.TipoGrafico.PIE:
                        return "pie-chart.png";
                    case AppEnums.TipoGrafico.RADAR:
                        return "radar-chart.png";
                    case AppEnums.TipoGrafico.LINE:
                        return "line-chart.png";
                    default:
                        return "line-chart.png";
                }
            }
        }
    }
}