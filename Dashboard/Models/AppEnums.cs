using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class AppEnums
    {
        public enum TipoGrafico
        {
            BAR,
            LINE,
            PIE,
            DOUGHNUT,
            RADAR
        }
        public enum DataOperation
        {
            SUM,
            AVG,
            COUNT
        }
        public enum ParamTipo
        {
            STRING,
            DATE,
            INT
        }
        public enum ParamTipoHtml
        {
            TEXT,
            DATE,
            SELECT
        }
        public enum ParamValorDefecto
        {
            EMPTY,
            ZERO,
            LASTMONTH,
            LASTWEEK,
            NOW
        }
        public enum ParamValidacion
        {
            NINGUNA,
            FECHA,
            RUT,
            NUMERO
        }
        public enum ParamValidacionCondicion
        {
            NINGUNA,
            MAYOR,
            MAYOROIGUAL,
            MENOR,
            MENOROIGUAL
        }

        public enum ParamValorMin
        {
            NINGUNA,
            ZERO,
            LASTWEEK,
            LASTMONTH,
            LASTYEAR,
            LAST2YEAR,
            LAST3YEAR,
            LAST4YEAR,
            LAST5YEAR
        }
        public enum ParamValorMax
        {
            NINGUNA,
            NOW,
            NEXTWEEK,
            NEXTMONTH,
            NEXTYEAR,
            NEXT2YEAR,
            NEXT3YEAR,
            NEXT4YEAR,
            NEXT5YEAR
        }
    }
}